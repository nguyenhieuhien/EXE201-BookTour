using EXE201.Models; // Đảm bảo namespace này được import

namespace EXE201.Service.Interface
{
    public interface IPackageServiceService
    {
        Task<EXE201.Models.PackageService> GetPackageServiceByIdAsync(long id);
        Task<IEnumerable<EXE201.Models.PackageService>> GetAllPackageServicesAsync();
        Task AddPackageServiceAsync(EXE201.Models.PackageService packageService);
        Task UpdatePackageServiceAsync(EXE201.Models.PackageService packageService);
        Task DeletePackageServiceAsync(long id);
    }
}
