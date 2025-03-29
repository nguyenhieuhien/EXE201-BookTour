using EXE201.Models;
using Repositories;

namespace Service
{
    public class SystemAccountService
    {
        private readonly SystemAccountRepository _repository;
        public SystemAccountService()
        {
            _repository = new SystemAccountRepository();
        }
        public async Task<Account> Authenticate(string userName, string password)
        {
            return await _repository.GetSystemAccount(userName, password);
        }
    }
}
