using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface ICartService
    {
        Task<Cart> GetCartByIdAsync(long id);
        Task<IEnumerable<Cart>> GetCartsByAccountIdAsync(long accountId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(long id);
    }

}
