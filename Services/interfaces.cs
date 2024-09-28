using Models;
namespace Services;

public interface IAttractionService
{
     Task ClearDatabaseAsync();
     public void RobustSeedAsync();
}
