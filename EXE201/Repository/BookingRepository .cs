using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EXE201.Controllers.DTO.Booking;
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

        public async Task RecalculateTotalPriceAsync(long bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null) return;

            var bookingDetails = await _context.BookingDetails
                .Where(bd => bd.BookingId == bookingId && bd.IsActive)
                .ToListAsync();

            if (bookingDetails == null || !bookingDetails.Any()) return;

            decimal total = 0;
            foreach (var detail in bookingDetails)
            {
                var package = await _context.Packages.FindAsync(detail.PackageId);
                if (package != null && package.IsActive)
                {
                    total += (decimal)package.Price;
                }
            }

            booking.TotalPrice = (double)total;

            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookingStatusAsync(long bookingId, string status)
        {
            var existingBooking = await _context.Bookings.FindAsync(bookingId);
            if (existingBooking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            existingBooking.Status = status;
            _context.Bookings.Update(existingBooking);
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
