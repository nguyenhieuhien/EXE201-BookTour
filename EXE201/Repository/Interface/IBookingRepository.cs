using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Controllers.DTO.Booking;
using EXE201.Models;

namespace EXE201.Repository.Interface
{
    public interface IBookingRepository
    {
        Task<IEnumerable<BookingDTO>> GetAllBookings();
        Task<BookingDTO?> GetBookingById(long id);
        Task AddBooking(Booking booking);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(long id);
    }
}
