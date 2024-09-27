using Configuration;
using Models;
using DbRepos;



using Seido.Utilities.SeedGenerator;

namespace Services;


public class csAttractionServiceDb: IAttractionService 
{

    private IAttractionRepo _repo = null;

    public void RobustSeedAsync () => _repo.RobustSeedAsync();

   

    public csAttractionServiceDb(IAttractionRepo repo)
    {
        _repo = repo;
    }
}