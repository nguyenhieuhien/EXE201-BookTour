
using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IAccountService
    {
        Task<Account> GetByIdAsync(long id);
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account> GetByNameAsync(string name);
        Task AddAsync(Account account);
        Task UpdateAsync(Account account);
        Task DeleteAsync(long id);
    }
}
