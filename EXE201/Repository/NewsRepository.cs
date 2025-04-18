using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly EXE201Context _context;

        public NewsRepository(EXE201Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _context.News.ToListAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _context.News.FindAsync(id);
        }

    }
}
