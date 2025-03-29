using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace EXE201.Repository
{
    public class PackageRepository : IPackageRepository
    {
        private readonly EXE201Context _context;

        public PackageRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Package> GetByIdAsync(long id)
        {
            return await _context.Packages.FindAsync(id);
        }

        public async Task<IEnumerable<Package>> GetAllAsync()
        {
            return await _context.Packages.ToListAsync();
        }

        public async Task AddAsync(Package package)
        {
            await _context.Packages.AddAsync(package);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Package package)
        {
            _context.Packages.Update(package);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package != null)
            {
                _context.Packages.Remove(package);
                await _context.SaveChangesAsync();
            }
        }
    }

}
