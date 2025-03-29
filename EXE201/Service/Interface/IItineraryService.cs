using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IItineraryService
    {
        Task<Itinerary> GetItineraryByIdAsync(long id);
        Task<IEnumerable<Itinerary>> GetAllItinerariesAsync();
        Task AddItineraryAsync(Itinerary itinerary);
        Task UpdateItineraryAsync(Itinerary itinerary);
        Task DeleteItineraryAsync(long id);
    }
}
