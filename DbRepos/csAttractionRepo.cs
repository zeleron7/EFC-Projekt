using Configuration;
using DbModels;
using DbContext;
using Seido.Utilities.SeedGenerator;
using Microsoft.EntityFrameworkCore;
using Models;

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
   
}