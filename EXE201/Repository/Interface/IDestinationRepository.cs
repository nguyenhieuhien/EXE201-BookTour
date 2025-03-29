using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IDestinationRepository
    {
        Task<Destination> GetByIdAsync(long id);
        Task<IEnumerable<Destination>> GetAllAsync();
        Task AddAsync(Destination destination);
        Task UpdateAsync(Destination destination);
        Task DeleteAsync(long id);
    }

}
