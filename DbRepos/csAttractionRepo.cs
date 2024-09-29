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
    public void  RobustSeedAsync()
    {
        var fn = Path.GetFullPath(_seedSource);
        var _seeder = new csSeedGenerator(fn);
        
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var users = _seeder.ItemsToList<csUserDbM>(50);
            var attractions = _seeder.ItemsToList<csAttractionDbM>(1000);
            var locations = _seeder.ItemsToList<csLocationDbM>(100); 


            var allComments = new List <csCommentDbM>();
            //var locations = new List<csLocationDbM>();

            

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

            db.SaveChangesAsync();

           

        }

    }

    
    //Method to delete data from database
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
                .Include(i => i.commentDbM)
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

        _query = _query.Where(a => a.commentDbM == null || !a.commentDbM.Any()); //TA BORT DENNA RAD?

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
                .Include(i => i.commentDbM)
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

        _query = _query.Where(a => a.commentDbM == null || !a.commentDbM.Any()); //TA BORT DENNA RAD?

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


}