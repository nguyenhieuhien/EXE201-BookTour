using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace EXE201.Repository
{
    public class DestinationRepository : IDestinationRepository
    {
        private readonly EXE201Context _context;

        public DestinationRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Destination> GetByIdAsync(long id)
        {
            return await _context.Destinations.FindAsync(id);
        }

        public async Task<IEnumerable<Destination>> GetAllAsync()
        {
            return await _context.Destinations.ToListAsync();
        }

        public async Task AddAsync(Destination destination)
        {
            await _context.Destinations.AddAsync(destination);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Destination destination)
        {
            _context.Destinations.Update(destination);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination != null)
            {
                _context.Destinations.Remove(destination);
                await _context.SaveChangesAsync();
            }
        }
    }
}
