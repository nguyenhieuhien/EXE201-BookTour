using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IPackageServiceRepository
    {
        Task<PackageService> GetByIdAsync(long id);
        Task<IEnumerable<PackageService>> GetAllAsync();
        Task AddAsync(PackageService packageService);
        Task UpdateAsync(PackageService packageService);
        Task DeleteAsync(long id);
    }

}
