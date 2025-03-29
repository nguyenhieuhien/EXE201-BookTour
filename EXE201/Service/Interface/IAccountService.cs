using EXE201.Controllers.DTO.EXE201.DTOs;

namespace EXE201.Service.Interface
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDTO>> GetAllAccountsAsync();
        Task<AccountDTO?> GetAccountByIdAsync(long id);
        Task<AccountDTO?> GetAccountByUsernameAsync(string username);
        Task AddAccountAsync(AccountDTO dto);
        Task UpdateAccountAsync(long id, AccountDTO dto);
        Task DeleteAccountAsync(long id);
    }
}
