using Configuration;
using Models;
using DbRepos;
using Models.DTO;


using Seido.Utilities.SeedGenerator;

namespace Services;


public class csAttractionServiceDb: IAttractionService 
{
    private IAttractionRepo _repo = null;

    public void RobustSeedAsync () => _repo.RobustSeedAsync();

    public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public csAttractionServiceDb(IAttractionRepo repo)
    {
        _repo = repo;
    }

    public async Task ClearDatabaseAsync()
    {
        // Call the repository method to clear the database
        await _repo.ClearDatabaseAsync();
    }
}