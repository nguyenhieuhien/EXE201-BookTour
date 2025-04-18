using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Controllers.DTO.Booking;
using EXE201.Models;

namespace EXE201.Service.Interface
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDTO>> GetAllBookings();
        Task<BookingDTO?> GetBookingById(long id);
        Task AddBooking(Booking booking);
        Task RecalculateTotalPriceAsync(long bookingId);
        Task UpdateBookingStatusAsync(long bookingId, string status);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(long id);
    }
}
