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

    public Task SeedDatabaseAsync () => _repo.SeedDatabaseAsync();

    public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadAttractionsAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadAttractionsWithoutCommentsAsync(seeded, flat, filter, pageNumber, pageSize);
    }

    public Task<csAttractionDbM> ReadOneAttractionAsync(Guid attractionId)
    {
        return _repo.ReadOneAttractionAsync(attractionId);
    }

    public Task<csRespPageDTO<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize)
    {
        return _repo.ReadUsersAsync(seeded, flat, filter, pageNumber, pageSize);
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