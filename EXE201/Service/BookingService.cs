using System.Collections.Generic;
using System.Threading.Tasks;
using EXE201.DTO;
using EXE201.Models;
using EXE201.Repository.Interface;
using EXE201.Service.Interface;

namespace EXE201.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<BookingDTO>> GetAllBookings()
        {
            return await _bookingRepository.GetAllBookings();
        }

        public async Task<BookingDTO?> GetBookingById(long id)
        {
            return await _bookingRepository.GetBookingById(id);
        }

        public async Task AddBooking(Booking booking)
        {
            await _bookingRepository.AddBooking(booking);
        }

        public async Task UpdateBooking(Booking booking)
        {
            await _bookingRepository.UpdateBooking(booking);
        }

        public async Task DeleteBooking(long id)
        {
            await _bookingRepository.DeleteBooking(id);
        }
    }
}
