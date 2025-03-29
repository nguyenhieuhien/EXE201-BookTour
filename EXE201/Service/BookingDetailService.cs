using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Models;
using EXE201.Repositories;

namespace EXE201.Service;

public class BookingDetailService : IBookingDetailService
{
    private readonly IBookingDetailRepository _bookingDetailRepository;

    public BookingDetailService(IBookingDetailRepository bookingDetailRepository)
    {
        _bookingDetailRepository = bookingDetailRepository;
    }

    public async Task<IEnumerable<BookingDetail>> GetAllBookingDetails()
    {
        return await _bookingDetailRepository.GetAllBookingDetails();
    }

    public async Task<BookingDetail?> GetBookingDetailById(long id)
    {
        return await _bookingDetailRepository.GetBookingDetailById(id);
    }

    public async Task<IEnumerable<BookingDetail>> GetBookingDetailsByBookingId(long bookingId)
    {
        return await _bookingDetailRepository.GetBookingDetailsByBookingId(bookingId);
    }

    public async Task AddBookingDetail(BookingDetail bookingDetail)
    {
        await _bookingDetailRepository.AddBookingDetail(bookingDetail);
    }

    public async Task UpdateBookingDetail(BookingDetail bookingDetail)
    {
        await _bookingDetailRepository.UpdateBookingDetail(bookingDetail);
    }

    public async Task DeleteBookingDetail(long id)
    {
        await _bookingDetailRepository.DeleteBookingDetail(id);
    }
}
