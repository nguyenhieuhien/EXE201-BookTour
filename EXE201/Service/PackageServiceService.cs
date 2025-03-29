using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;
namespace EXE201.Service
{
    public class PackageServiceService : IPackageServiceService
    {
        private readonly IPackageServiceRepository _packageServiceRepository;

        public PackageServiceService(IPackageServiceRepository packageServiceRepository)
        {
            _packageServiceRepository = packageServiceRepository;
        }

        public async Task<EXE201.Models.PackageService> GetPackageServiceByIdAsync(long id)
        {
            return await _packageServiceRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<EXE201.Models.PackageService>> GetAllPackageServicesAsync()
        {
            return await _packageServiceRepository.GetAllAsync();
        }

        public async Task AddPackageServiceAsync(EXE201.Models.PackageService packageService)
        {
            await _packageServiceRepository.AddAsync(packageService);
        }

        public async Task UpdatePackageServiceAsync(EXE201.Models.PackageService packageService)
        {
            await _packageServiceRepository.UpdateAsync(packageService);
        }

        public async Task DeletePackageServiceAsync(long id)
        {
            await _packageServiceRepository.DeleteAsync(id);
        }
    }
}
