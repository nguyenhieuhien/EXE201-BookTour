using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IPackageService
    {
        Task<Package> GetPackageByIdAsync(long id);
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task AddPackageAsync(Package package);
        Task UpdatePackageAsync(Package package);
        Task DeletePackageAsync(long id);
    }

}
