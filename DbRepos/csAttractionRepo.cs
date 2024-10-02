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
            var allComments = new List<csCommentDbM>();

            foreach (var attraction in attractions)
            {
                var newLocation = new csLocationDbM().Seed(_seeder);

                //attraction.CommentDbM = _seeder.ItemsToList<csCommentDbM>(_seeder.Next(0,21)); //TA bort denna rad?

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
            db.Comments.RemoveRange(db.Comments); 
            db.Attractions.RemoveRange(db.Attractions); 
            db.Locations.RemoveRange(db.Locations); 
            db.Users.RemoveRange(db.Users); 

            await db.SaveChangesAsync();
        }
    }

    //Method to read attractions
    public async Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
    using (var db = csMainDbContext.DbContext("sysadmin"))
    {
        IQueryable<csAttractionDbM> _query;

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

        var totalItems = await _query.CountAsync();
        var items = await _query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync<IAttraction>();

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

        if (flat)
        {
            //_query = db.Attractions.AsNoTracking();
            _query = db.Attractions.Include(a => a.CommentDbM).Where(a => a.CommentDbM == null).AsNoTracking();

        }
        else
        {
            _query = db.Attractions.Include(a => a.CommentDbM).Where(a => a.CommentDbM == null).AsNoTracking()
                .Include(i => i.LocationDbM);
        }

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

        //_query = _query.Where(a => a.CommentDbM == null || !a.CommentDbM.Any()); 
        

        var totalItems = await _query.CountAsync();
        var items = await _query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync<IAttraction>();

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
        var attraction = await db.Attractions
            .AsNoTracking()
            .Include(a => a.CommentDbM)   
            .Include(a => a.LocationDbM)  
            .Where(a =>  a.AttractionId == attractionId)
            .FirstOrDefaultAsync();

        if (attraction == null)
        {
            return null; 
        }

        return attraction; 
    }
    }

    //Method to read users and their comments
    public async Task<csRespPageDTO<Models.IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
    {
        IQueryable<csUserDbM> _query;

        // Base query for flat or nested includes
        if (flat)
        {
            _query = db.Users.AsNoTracking();
        }
        else
        {
            _query = db.Users.AsNoTracking()
                .Include(i => i.CommentDbM);
        }

        // Filtering by category, title, description, country, and city
        if (!string.IsNullOrEmpty(filter))
        {
            _query = _query.Where(a =>
                (a.FirstName != null && a.FirstName.Contains(filter)) ||
                (a.LastName != null && a.LastName.Contains(filter)) 
            );
        }



        // Pagination (skip and take)
        var totalItems = await _query.CountAsync();
        var items = await _query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync<Models.IUser>();

        // Returning paginated response
        var _ret = new csRespPageDTO<Models.IUser>
        {
            PageItems = items,
            DbItemsCount = totalItems,
            PageNr = pageNumber,
            PageSize = pageSize
        };

        return _ret;
    }
    }
}