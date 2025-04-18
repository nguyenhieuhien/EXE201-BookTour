using EXE201.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository.Interface
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News> GetByIdAsync(int id);
       
    }
}
