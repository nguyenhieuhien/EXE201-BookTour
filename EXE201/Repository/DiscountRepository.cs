using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly EXE201Context _context;

        public DiscountRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Discount> GetByIdAsync(long id)
        {
            return await _context.Discounts.FindAsync(id);
        }

        public async Task<IEnumerable<Discount>> GetAllAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<Discount> GetByCodeAsync(string code)
        {
            return await _context.Discounts.FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount != null)
            {
                _context.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
        }
    }
}
