using DbModels;
using Models;
using Models.DTO;
namespace DbRepos;

public interface IAttractionRepo
{
     public Task ClearDatabaseAsync();
     public void RobustSeedAsync();

     //public Task ReadAttractionsAsync();
    Task<csRespPageDTO<IAttraction>> ReadAttractionsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);

    Task<csRespPageDTO<IAttraction>> ReadAttractionsWithoutCommentsAsync(bool seeded, bool flat, string filter, int pageNumber, int pageSize);
}