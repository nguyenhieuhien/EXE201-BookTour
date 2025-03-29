using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IDiscountRepository
    {
        Task<Discount> GetByIdAsync(long id);
        Task<IEnumerable<Discount>> GetAllAsync();
        Task<Discount> GetByCodeAsync(string code);
        Task AddAsync(Discount discount);
        Task UpdateAsync(Discount discount);
        Task DeleteAsync(long id);
    }
}
