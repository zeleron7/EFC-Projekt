using DbModels;
namespace DbRepos;

public interface IAnimalRepo
{
    public Task<List<csAnimalDbM>> AfricanAnimals(int _count);
    public Task Seed(int _count);
}