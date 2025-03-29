using EXE201.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE201.Service.Interface
{
    public interface IServiceService
    {
        Task<Models.Service> GetByIdAsync(long id);
        Task<IEnumerable<Models.Service>> GetAllAsync();
        Task AddAsync(Models.Service service);
        Task UpdateAsync(Models.Service service);
        Task DeleteAsync(long id);
        Task<Models.Service?> GetByNameAsync(string name);
    }
}
