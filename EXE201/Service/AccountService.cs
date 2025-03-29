using EXE201.Models;
using Microsoft.EntityFrameworkCore;
using EXE201.Service.Interface;
using EXE201.Controllers.DTO.EXE201.DTOs;

namespace EXE201.Service
{
    public class AccountService : IAccountService
    {
        private readonly EXE201Context _context;

        public AccountService(EXE201Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountDTO>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Select(a => new AccountDTO
                {
                  
                    UserName = a.UserName,
                    Email = a.Email,
                    Phone = a.Phone,
                    IsActive = a.IsActive,
                    RoleId = a.RoleId
                })
                .ToListAsync();
        }

        public async Task<AccountDTO?> GetAccountByIdAsync(long id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return null;

            return new AccountDTO
            {
                
                UserName = account.UserName,
                Email = account.Email,
                Phone = account.Phone,
                IsActive = account.IsActive,
                RoleId = account.RoleId
            };
        }

        public async Task<AccountDTO?> GetAccountByUsernameAsync(string username)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserName == username);
            if (account == null) return null;

            return new AccountDTO
            {
              
                UserName = account.UserName,
                Email = account.Email,
                Phone = account.Phone,
                IsActive = account.IsActive,
                RoleId = account.RoleId
            };
        }

        public async Task AddAccountAsync(AccountDTO dto)
        {
            var account = new Account
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Phone = dto.Phone,
                Password = dto.Password, // Hash the password
                RoleId = dto.RoleId,
                IsActive = true
            };

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountAsync(long id, AccountDTO dto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return;

            account.UserName = dto.UserName;
            account.Email = dto.Email;
            account.Phone = dto.Phone;
            account.IsActive = dto.IsActive;  // ✅ Fixed this line
            account.RoleId = dto.RoleId;

            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountAsync(long id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
