using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class ItineraryService : IItineraryService
    {
        private readonly IItineraryRepository _itineraryRepository;

        public ItineraryService(IItineraryRepository itineraryRepository)
        {
            _itineraryRepository = itineraryRepository;
        }

        public async Task<Itinerary> GetItineraryByIdAsync(long id)
        {
            return await _itineraryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Itinerary>> GetAllItinerariesAsync()
        {
            return await _itineraryRepository.GetAllAsync();
        }

        public async Task AddItineraryAsync(Itinerary itinerary)
        {
            await _itineraryRepository.AddAsync(itinerary);
        }

        public async Task UpdateItineraryAsync(Itinerary itinerary)
        {
            await _itineraryRepository.UpdateAsync(itinerary);
        }

        public async Task DeleteItineraryAsync(long id)
        {
            await _itineraryRepository.DeleteAsync(id);
        }
    }
}
