using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace EXE201.Repository
{
    public class PackageServiceRepository : IPackageServiceRepository
    {
        private readonly EXE201Context _context;

        public PackageServiceRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<PackageService> GetByIdAsync(long id)
        {
            return await _context.PackageServices.FindAsync(id);
        }

        public async Task<IEnumerable<PackageService>> GetAllAsync()
        {
            return await _context.PackageServices.ToListAsync();
        }

        public async Task AddAsync(PackageService packageService)
        {
            await _context.PackageServices.AddAsync(packageService);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PackageService packageService)
        {
            _context.PackageServices.Update(packageService);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var packageService = await _context.PackageServices.FindAsync(id);
            if (packageService != null)
            {
                _context.PackageServices.Remove(packageService);
                await _context.SaveChangesAsync();
            }
        }
    }

}
