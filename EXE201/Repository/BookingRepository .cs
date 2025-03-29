using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EXE201.DTO;

using EXE201.Models;
using EXE201.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EXE201.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly EXE201Context _context;

        public BookingRepository(EXE201Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            return await _context.Bookings
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    AccountId = b.AccountId,
                    DiscountId = b.DiscountId,
                    Description = b.Description,
                    BookingDate = b.BookingDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status
                })
                .ToListAsync();
        }

        public async Task<BookingDTO?> GetBookingById(long id)
        {
            return await _context.Bookings
                .Where(b => b.Id == id)
                .Select(b => new BookingDTO
                {
                    Id = b.Id,
                    AccountId = b.AccountId,
                    DiscountId = b.DiscountId,
                    Description = b.Description,
                    BookingDate = b.BookingDate,
                    TotalPrice = b.TotalPrice,
                    Status = b.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddBooking(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBooking(long id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
    }
}
