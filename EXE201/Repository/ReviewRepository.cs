using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly EXE201Context _context;

        public ReviewRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<Review> GetByIdAsync(long id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();

        }

        //public async Task<IEnumerable<Review>> GetAllAsync()
        //{
        //    return await _context.Reviews
        //.Include(r => r.Account)
        //.Include(r => r.Package)
        //.ToListAsync();
        //}
        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
