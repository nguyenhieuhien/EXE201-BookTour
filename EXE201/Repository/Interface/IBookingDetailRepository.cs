using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Models;

namespace EXE201.Repositories;

public interface IBookingDetailRepository
{
    Task<IEnumerable<BookingDetail>> GetAllBookingDetails();
    Task<BookingDetail?> GetBookingDetailById(long id);
    Task<IEnumerable<BookingDetail>> GetBookingDetailsByBookingId(long bookingId);
    Task AddBookingDetail(BookingDetail bookingDetail);
    Task UpdateBookingDetail(BookingDetail bookingDetail);
    Task DeleteBookingDetail(long id);
}
