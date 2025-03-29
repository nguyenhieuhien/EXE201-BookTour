using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository
{
    public class ItineraryRepository : IItineraryRepository
    {
        private readonly EXE201Context _context;

        public ItineraryRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Itinerary> GetByIdAsync(long id)
        {
            return await _context.Itineraries.FindAsync(id);
        }

        public async Task<IEnumerable<Itinerary>> GetAllAsync()
        {
            return await _context.Itineraries.ToListAsync();
        }

        public async Task AddAsync(Itinerary itinerary)
        {
            await _context.Itineraries.AddAsync(itinerary);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Itinerary itinerary)
        {
            _context.Itineraries.Update(itinerary);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var itinerary = await _context.Itineraries.FindAsync(id);
            if (itinerary != null)
            {
                _context.Itineraries.Remove(itinerary);
                await _context.SaveChangesAsync();
            }
        }
    }
}
