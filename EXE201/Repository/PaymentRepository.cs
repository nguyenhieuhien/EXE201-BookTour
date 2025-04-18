using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly EXE201Context _context;

        public PaymentRepository(EXE201Context context)
        {
            _context = context;
        }
        public async Task<Payment> GetByOrderCodeAsync(long? orderCode)
        {
            if (orderCode == null)
            {
                return null; // Hoặc throw new ArgumentNullException(nameof(orderCode));
            }
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderCode == orderCode);
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                Console.WriteLine($"Payment updated successfully for orderCode: {payment.OrderCode}");
            }
            else
            {
                Console.WriteLine($"Failed to update payment for orderCode: {payment.OrderCode}");
            }
        }


    }
}
