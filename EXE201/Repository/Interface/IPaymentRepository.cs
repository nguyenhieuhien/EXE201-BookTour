using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByOrderCodeAsync(long? orderCode);
        Task CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
    }

}
