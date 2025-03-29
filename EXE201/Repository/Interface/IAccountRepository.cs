using EXE201.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE201.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(long id);
        Task<Account?> GetAccountByUsernameAsync(string username);
        Task AddAccountAsync(Account account);
        Task UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(long id);
    }
}
