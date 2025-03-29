using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IReviewService
    {
        Task<Review> GetReviewByIdAsync(long id);
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(long id);
    }
}
