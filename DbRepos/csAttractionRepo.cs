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
            var cities = new List<csCitiesDbM>();
            //var countries = new List<csCountryDbM>();

            

            foreach (var attraction in attractions)
            {
                var newLocation = new csCitiesDbM().Seed(_seeder);

                var exsistLocation = cities.FirstOrDefault(l => l.Country == newLocation.Country && l.Country == newLocation.Country)
                ?? db.Cities.FirstOrDefault(l => l.Country == newLocation.Country && l.Country == newLocation.Country);


                if (exsistLocation == null)
                {
                    cities.Add(newLocation);
                    attraction.CitiesDbM = newLocation;
                }
                else
                {
                    attraction.CitiesDbM = exsistLocation;
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

           

            db.Cities.AddRange(cities);
            db.Users.AddRange(users);
            db.Attractions.AddRange(attractions);
            db.Comments.AddRange(allComments);

            db.SaveChangesAsync();


           

        }
    }
   
}