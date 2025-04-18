using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IPaymentService
    {
       Task<Payment> GetByOrderCodeAsync(long? orderCode);
        Task CreatePaymentAsync(Payment payment);
       Task UpdatePaymentAsync(Payment payment);
    }
}
