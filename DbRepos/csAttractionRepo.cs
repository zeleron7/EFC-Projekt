using Configuration;
using DbModels;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using Microsoft.Identity.Client;

namespace DbRepos;

public class csAttractionRepo : IAttractionRepo
{
    const string _seedSource = "./friends-seeds1.json";

    //Method to seed database
    public async Task SeedDatabaseAsync()
    {
        var fn = Path.GetFullPath(_seedSource);
        var _seeder = new csSeedGenerator(fn);
        
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var users = _seeder.ItemsToList<csUserDbM>(50);
            var attractions = _seeder.ItemsToList<csAttractionDbM>(1000);
            var locations = _seeder.ItemsToList<csLocationDbM>(100); 
            var allComments = new List <csCommentDbM>();
            
            foreach (var attraction in attractions)
            {
                var newLocation = new csLocationDbM().Seed(_seeder);

                var exsistLocation = locations.FirstOrDefault(l => l.Country == newLocation.Country && l.City == newLocation.City)
                ?? db.Locations.FirstOrDefault(l => l.Country == newLocation.Country && l.City == newLocation.City);

                if (exsistLocation == null)
                {
                    locations.Add(newLocation);
                    attraction.LocationDbM = newLocation;
                }
                else
                {
                    attraction.LocationDbM = exsistLocation;
                }

                int nmrOfComments = _seeder.Next(0, 21);
                if (nmrOfComments > 0)
                {
                    for (int i = 0; i < nmrOfComments; i++)
                    {
                       var comment = new csCommentDbM().Seed(_seeder);
                       comment.UserDbM = _seeder.FromList(users);
                       comment.AttractionDbM = attraction;

                       allComments.Add(comment);
                    }
                }   
            }
           
            db.Locations.AddRange(locations);
            db.Users.AddRange(users);
            db.Attractions.AddRange(attractions);
            db.Comments.AddRange(allComments);

            await db.SaveChangesAsync();

            

        }
    }
    
    //Method to clear database
    public async Task ClearDatabaseAsync()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            // Remove all data in the right order to handle foreign key constraints
            db.Comments.RemoveRange(db.Comments); // Delete comments first
            db.Attractions.RemoveRange(db.Attractions); // Then attractions
            db.Locations.RemoveRange(db.Locations); // Then locations
            db.Users.RemoveRange(db.Users); // Finally, users

            // Save changes to database
            await db.SaveChangesAsync();
        }
    }

    //Method to read attractions
    public async Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
    using (var db = csMainDbContext.DbContext("sysadmin"))
    {
        IQueryable<csAttractionDbM> _query;

        // Base query for flat or nested includes
        if (flat)
        {
            _query = db.Attractions.AsNoTracking();
        }
        else
        {
            _query = db.Attractions.AsNoTracking()
                .Include(i => i.CommentDbM)
                .Include(i => i.LocationDbM);
                
        }

        // Filtering by category, title, description, country, and city
        if (!string.IsNullOrEmpty(filter))
        {
            _query = _query.Where(a =>
                (a.Category != null && a.Category.Contains(filter)) ||
                (a.Title != null && a.Title.Contains(filter)) ||
                (a.Description != null && a.Description.Contains(filter)) ||
                (a.LocationDbM != null && a.LocationDbM.Country != null && a.LocationDbM.Country.Contains(filter)) ||
                (a.LocationDbM != null && a.LocationDbM.City != null && a.LocationDbM.City.Contains(filter))
            );
        }



        // Pagination (skip and take)
        var totalItems = await _query.CountAsync();
        var items = await _query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync<IAttraction>();

        // Returning paginated response
        var _ret = new csRespPageDTO<IAttraction>
        {
            PageItems = items,
            DbItemsCount = totalItems,
            PageNr = pageNumber,
            PageSize = pageSize
        };

        return _ret;
    }
    }

    //Method to read attraction without comments
    public async Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
    using (var db = csMainDbContext.DbContext("sysadmin"))
    {
        IQueryable<csAttractionDbM> _query;

        // Base query for flat or nested includes
        if (flat)
        {
            _query = db.Attractions.AsNoTracking();
        }
        else
        {
            _query = db.Attractions.AsNoTracking()
                .Include(i => i.CommentDbM)
                .Include(i => i.LocationDbM);
        }

        // Filtering by category, title, description, country, and city
        if (!string.IsNullOrEmpty(filter))
        {
            _query = _query.Where(a =>
                (a.Category != null && a.Category.Contains(filter)) ||
                (a.Title != null && a.Title.Contains(filter)) ||
                (a.Description != null && a.Description.Contains(filter)) ||
                (a.LocationDbM != null && a.LocationDbM.Country != null && a.LocationDbM.Country.Contains(filter)) ||
                (a.LocationDbM != null && a.LocationDbM.City != null && a.LocationDbM.City.Contains(filter))
            );
        }

        _query = _query.Where(a => a.CommentDbM == null || !a.CommentDbM.Any()); //TA BORT DENNA RAD?

        // Pagination (skip and take)
        var totalItems = await _query.CountAsync();
        var items = await _query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync<IAttraction>();

        // Returning paginated response
        var _ret = new csRespPageDTO<IAttraction>
        {
            PageItems = items,
            DbItemsCount = totalItems,
            PageNr = pageNumber,
            PageSize = pageSize
        };

        return _ret;
    }
    }

    //Method to read one attraction
    public async Task<csAttractionDbM> ReadOneAttractionAsync(Guid attractionId)
    {
    using (var db = csMainDbContext.DbContext("sysadmin"))
    {
        // Query to get a single attraction by ID and include its comments and location
        var attraction = await db.Attractions
            .AsNoTracking()
            .Include(a => a.CommentDbM)   // Include comments
            .Include(a => a.LocationDbM)  // Optionally include location data
            .Where(a =>  a.AttractionId == attractionId)
            .FirstOrDefaultAsync();

        // If no attraction is found, return null
        if (attraction == null)
        {
            return null; // Or throw an exception if needed
        }

        return attraction; // Return the entire attraction entity with comments and location included
    }
    }

    //Method to read users and their comments
    public async Task<csUserDbM> ReadUsers()
    {
    using (var db = csMainDbContext.DbContext("sysadmin"))
    {
        // Fetch all users with their comments using eager loading (Include)
        var usersWithComments = await db.Users
            .Include(u => u.CommentDbM)  // Eagerly load related comments
            .FirstOrDefaultAsync();

        // Convert to csUserDbM model (including user data and their comments)
       

        return usersWithComments;
    }
    }
}