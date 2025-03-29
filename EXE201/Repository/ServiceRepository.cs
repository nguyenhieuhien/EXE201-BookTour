using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE201.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly EXE201Context _context;

        public ServiceRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Models.Service> GetByIdAsync(long id)
        {
            return await _context.Services.FindAsync(id);
        }

        public async Task<IEnumerable<Models.Service>> GetAllAsync()
        {
            return await _context.Services.ToListAsync();
        }
        public async Task<Models.Service?> GetByNameAsync(string name)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task AddAsync(Models.Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Models.Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}
