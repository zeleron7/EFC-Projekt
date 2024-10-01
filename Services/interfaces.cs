using Models;
namespace Services;
using Models.DTO;
using DbModels;

public interface IAttractionService
{
     public Task ClearDatabaseAsync();
     public Task SeedDatabaseAsync();
     public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

     public Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

     public Task<csAttractionDbM> ReadOneAttractionAsync(Guid attractionId);

     public Task<csUserDbM> ReadUsers();
}
