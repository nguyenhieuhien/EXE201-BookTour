using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE201.Service
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public async Task<CartItem> GetByIdAsync(long id)
        {
            return await _cartItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(long cartId)
        {
            return await _cartItemRepository.GetByCartIdAsync(cartId);
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _cartItemRepository.AddAsync(cartItem);
        }

        public async Task UpdateAsync(CartItem cartItem)
        {
            await _cartItemRepository.UpdateAsync(cartItem);
        }

        public async Task DeleteAsync(long id)
        {
            await _cartItemRepository.DeleteAsync(id);
        }
    }
}
