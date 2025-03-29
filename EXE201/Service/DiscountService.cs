using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<Discount> GetDiscountByIdAsync(long id)
        {
            return await _discountRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
        {
            return await _discountRepository.GetAllAsync();
        }

        public async Task<Discount> GetDiscountByCodeAsync(string code)
        {
            return await _discountRepository.GetByCodeAsync(code);
        }

        public async Task AddDiscountAsync(Discount discount)
        {
            await _discountRepository.AddAsync(discount);
        }

        public async Task UpdateDiscountAsync(Discount discount)
        {
            await _discountRepository.UpdateAsync(discount);
        }

        public async Task DeleteDiscountAsync(long id)
        {
            await _discountRepository.DeleteAsync(id);
        }
    }
}
