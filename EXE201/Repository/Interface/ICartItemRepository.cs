using EXE201.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE201.Repository.Interface
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetByIdAsync(long id);
        Task<IEnumerable<CartItem>> GetByCartIdAsync(long cartId);
        Task AddAsync(CartItem cartItem);
        Task UpdateAsync(CartItem cartItem);
        Task DeleteAsync(long id);
    }
}
