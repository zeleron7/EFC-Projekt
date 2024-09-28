using DbModels;
namespace DbRepos;

public interface IAttractionRepo
{
     public Task ClearDatabaseAsync();
     public void RobustSeedAsync();
  
}