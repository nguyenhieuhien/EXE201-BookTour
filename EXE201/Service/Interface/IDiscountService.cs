using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IDiscountService
    {
        Task<Discount> GetDiscountByIdAsync(long id);
        Task<IEnumerable<Discount>> GetAllDiscountsAsync();
        Task<Discount> GetDiscountByCodeAsync(string code);
        Task AddDiscountAsync(Discount discount);
        Task UpdateDiscountAsync(Discount discount);
        Task DeleteDiscountAsync(long id);
    }
}
