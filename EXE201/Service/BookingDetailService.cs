using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.Models;
using EXE201.Repositories;
using EXE201.Service.Interface;

namespace EXE201.Service;

public class BookingDetailService : IBookingDetailService
{
    private readonly IBookingDetailRepository _bookingDetailRepository;
    private readonly IBookingService _bookingService;


    public BookingDetailService(IBookingDetailRepository bookingDetailRepository, IBookingService bookingService)
    {
        _bookingDetailRepository = bookingDetailRepository;
        _bookingService = bookingService;
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
        await _bookingService.RecalculateTotalPriceAsync(bookingDetail.BookingId);
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
