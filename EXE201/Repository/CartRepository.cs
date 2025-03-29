using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;

namespace EXE201.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly EXE201Context _context;

        public CartRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Cart> GetByIdAsync(long id)
        {
            return await _context.Carts.FindAsync(id);
        }

        public async Task<IEnumerable<Cart>> GetByAccountIdAsync(long accountId)
        {
            return await _context.Carts.Where(c => c.AccountId == accountId).ToListAsync();
        }

        public async Task AddAsync(Cart cart)
        {
            cart.Id = 0;
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
    }

}
