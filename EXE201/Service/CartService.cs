using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<Cart> GetCartByIdAsync(long id)
        {
            return await _cartRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cart>> GetCartsByAccountIdAsync(long accountId)
        {
            return await _cartRepository.GetByAccountIdAsync(accountId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _cartRepository.AddAsync(cart);
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            await _cartRepository.UpdateAsync(cart);
        }

        public async Task DeleteCartAsync(long id)
        {
            await _cartRepository.DeleteAsync(id);
        }
    }

}
