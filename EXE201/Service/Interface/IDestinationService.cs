using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IDestinationService
    {
        Task<Destination> GetDestinationByIdAsync(long id);
        Task<IEnumerable<Destination>> GetAllDestinationsAsync();
        Task AddDestinationAsync(Destination destination);
        Task UpdateDestinationAsync(Destination destination);
        Task DeleteDestinationAsync(long id);
    }

}
