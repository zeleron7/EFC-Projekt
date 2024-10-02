using DbModels;
using Models;
using Models.DTO;
namespace DbRepos;

public interface IAttractionRepo
{
    public Task ClearDatabaseAsync();
    public Task SeedDatabaseAsync();
    Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
    public Task<csAttractionDbM> ReadOneAttractionAsync(Guid attractionId);
    public  Task<csRespPageDTO<IUser>> ReadUsersAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
}