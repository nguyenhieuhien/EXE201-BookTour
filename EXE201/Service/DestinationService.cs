using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;

        public DestinationService(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }

        public async Task<Destination> GetDestinationByIdAsync(long id)
        {
            return await _destinationRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Destination>> GetAllDestinationsAsync()
        {
            return await _destinationRepository.GetAllAsync();
        }

        public async Task AddDestinationAsync(Destination destination)
        {
            await _destinationRepository.AddAsync(destination);
        }

        public async Task UpdateDestinationAsync(Destination destination)
        {
            await _destinationRepository.UpdateAsync(destination);
        }

        public async Task DeleteDestinationAsync(long id)
        {
            await _destinationRepository.DeleteAsync(id);
        }
    }
}

