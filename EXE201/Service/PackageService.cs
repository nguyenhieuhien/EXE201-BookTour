using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;

        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<Package> GetPackageByIdAsync(long id)
        {
            return await _packageRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _packageRepository.GetAllAsync();
        }

        public async Task AddPackageAsync(Package package)
        {
            await _packageRepository.AddAsync(package);
        }

        public async Task UpdatePackageAsync(Package package)
        {
            await _packageRepository.UpdateAsync(package);
        }

        public async Task DeletePackageAsync(long id)
        {
            await _packageRepository.DeleteAsync(id);
        }
    }

}
