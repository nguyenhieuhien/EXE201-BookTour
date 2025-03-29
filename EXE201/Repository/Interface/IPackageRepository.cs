using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IPackageRepository
    {
        Task<Package> GetByIdAsync(long id);
        Task<IEnumerable<Package>> GetAllAsync();
        Task AddAsync(Package package);
        Task UpdateAsync(Package package);
        Task DeleteAsync(long id);
    }

}
