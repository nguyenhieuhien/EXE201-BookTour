using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface ICartRepository
    {
        Task<Cart> GetByIdAsync(long id);
        Task<IEnumerable<Cart>> GetByAccountIdAsync(long accountId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(long id);
    }
}
