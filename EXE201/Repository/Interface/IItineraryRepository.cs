using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IItineraryRepository
    {
        Task<Itinerary> GetByIdAsync(long id);
        Task<IEnumerable<Itinerary>> GetAllAsync();
        Task AddAsync(Itinerary itinerary);
        Task UpdateAsync(Itinerary itinerary);
        Task DeleteAsync(long id);
    }
}
