using EXE201.Models;
using EXE201.Repository;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public async Task<Payment> GetByOrderCodeAsync(long? orderCode)
        {
            return await _paymentRepository.GetByOrderCodeAsync(orderCode);
        }
        public async Task CreatePaymentAsync(Payment payment)
        {
            if (payment == null)
            {
                throw new ArgumentNullException(nameof(payment));
            }

            var existingPayment = await _paymentRepository.GetByOrderCodeAsync(payment.OrderCode);
            if (existingPayment != null)
            {
                throw new InvalidOperationException($"Payment with OrderCode {payment.OrderCode} already exists.");
            }

            await _paymentRepository.CreatePaymentAsync(payment);
        }
        public async Task UpdatePaymentAsync(Payment payment)
        {
           await _paymentRepository.UpdatePaymentAsync(payment);
        }
    }
}
