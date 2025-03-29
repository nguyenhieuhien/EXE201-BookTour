using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(long id);
        Task<IEnumerable<Review>> GetAllAsync();
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(long id);
    }
}
