using Configuration;
using Models;
using DbRepos;
using Models.DTO;
using DbModels;

using Seido.Utilities.SeedGenerator;

namespace Services;


public class csAttractionServiceDb: IAttractionService 
{
    private IAttractionRepo _repo = null;

    public void SeedDatabaseAsync () => _repo.SeedDatabaseAsync();

    public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public Task<csAttractionDbM> ReadOneAttractionAsync(Guid attractionId)
    {
        return _repo.ReadOneAttractionAsync(attractionId);
    }

    public Task<csUserDbM> ReadUsers()
    {
        return _repo.ReadUsers();
    }

    public csAttractionServiceDb(IAttractionRepo repo)
    {
        _repo = repo;
    }

    public async Task ClearDatabaseAsync()
    {
        await _repo.ClearDatabaseAsync();
    }
}