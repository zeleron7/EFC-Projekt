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
            


            var allComments = new List <csCommentDbM>();
            var locations = new List<csLocationDbM>();

            

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



    // Method to filter attractions by category, title, description, country, and city
    /*public async Task<List<csAttractionDbM>> GetFilteredAttractionsAsync(string? category, string? title, string? description, string? country, string? city)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var query = db.Attractions.Include(a => a.LocationDbM).AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(a => a.Category == category);
            }
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(a => a.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(a => a.Description.Contains(description));
            }
            if (!string.IsNullOrEmpty(country))
            {
                query = query.Where(a => a.LocationDbM.Country == country);
            }
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(a => a.LocationDbM.City == city);
            }

            return await query.ToListAsync();
        }
    }

    // Method to show all attractions without any comments
    public async Task<List<csAttractionDbM>> GetAttractionsWithoutCommentsAsync()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var attractionsWithoutComments = await db.Attractions
                .Include(a => a.Comments)
                .Where(a => !a.Comments.Any())
                .ToListAsync();

            return attractionsWithoutComments;
        }
    }

    // Method to show one attraction's category, title, description, and all associated comments
    public async Task<csAttractionDbM?> GetAttractionWithCommentsAsync(int attractionId)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var attraction = await db.Attractions
                .Include(a => a.Comments)
                .ThenInclude(c => c.UserDbM) // Including the users who wrote the comments
                .FirstOrDefaultAsync(a => a.AttractionId == attractionId);

            return attraction;
        }
    }

    // Method to show all users and their comments
    public async Task<List<csUserDbM>> GetUsersAndCommentsAsync()
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var usersWithComments = await db.Users
                .Include(u => u.Comments)
                .ThenInclude(c => c.AttractionDbM) // Including the attractions on which the users commented
                .ToListAsync();

            return usersWithComments;
        }
    }*/
   

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
                    .Include(i => i.commentDbM)
                    .Include(i => i.LocationDbM);
                    
            }
            
            var _ret = new csRespPageDTO<IAttraction>()
            {
                PageItems = await _query.ToListAsync<IAttraction>(),
            };
            return _ret;
        }
    }

}