using Microsoft.EntityFrameworkCore;

using Configuration;
using DbModels;
using DbContext;
using Seido.Utilities.SeedGenerator;

namespace DbRepos;

public class csAnimalRepo : IAnimalRepo
{

    private const string seedSource = "./friends-seeds1.json";

    public async Task<List<csAnimalDbM>> AfricanAnimals(int _count)
    {
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            return await db.Animals.Include(a => a.ZooDbM).Take(_count).ToListAsync();
        }
    }
    public async Task Seed(int _count)
    {
        var fn = Path.GetFullPath(seedSource);
        var _seeder = new csSeedGenerator(fn);
        using (var db = csMainDbContext.DbContext("sysadmin"))
        {
            var zoos = _seeder.ItemsToList<csZooDbM>(5);
            var animals = _seeder.ItemsToList<csAnimalDbM>(_count);

            foreach (var a in animals)
            {
                a.ZooDbM = _seeder.FromList(zoos);
            }
            
            
            db.Animals.AddRange(animals);
            await db.SaveChangesAsync();
        }
    }
}
