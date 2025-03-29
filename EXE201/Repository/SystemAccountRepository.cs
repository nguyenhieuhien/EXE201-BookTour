using EXE201.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Base;

namespace Repositories
{
    public class SystemAccountRepository : GenericRepository<Account>
    {
        public SystemAccountRepository() { }

        public async Task<Account> GetSystemAccount(string userName, string password)
        {
            return await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
        }
    }
}
