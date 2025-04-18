using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _newsRepository.GetAllAsync();
        }

        public async Task<News> GetByIdAsync(int id)
        {
            return await _newsRepository.GetByIdAsync(id);
        }
    }
}
