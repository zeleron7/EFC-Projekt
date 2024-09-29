using Models;
namespace Services;
using Models.DTO;

public interface IAttractionService
{
     Task ClearDatabaseAsync();
     public void RobustSeedAsync();
     public Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

     public Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
}
