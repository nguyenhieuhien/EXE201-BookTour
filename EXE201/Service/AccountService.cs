using EXE201.Models;
using Microsoft.EntityFrameworkCore;
using EXE201.Service.Interface;
using EXE201.Repository.Interface;
using EXE201.Repositories;

namespace EXE201.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> GetByIdAsync(long id)
        {
            return await _accountRepository.GetAccountByIdAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account> GetByNameAsync(string name)
        {
            return await _accountRepository.GetAccountByUsernameAsync(name);
        }

        public async Task AddAsync(Account account)
        {
            await _accountRepository.AddAccountAsync(account);
        }

        public async Task UpdateAsync(Account account)
        {
            await _accountRepository.UpdateAccountAsync(account);
        }

        public async Task DeleteAsync(long id)
        {
            await _accountRepository.DeleteAccountAsync(id);
        }
    }
}
