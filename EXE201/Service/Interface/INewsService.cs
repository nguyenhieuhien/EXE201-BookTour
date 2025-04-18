using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News> GetByIdAsync(int id);
    }
}
