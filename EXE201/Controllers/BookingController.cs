
using EXE201.DTO;

using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var booking = await _bookingService.GetBookingById(id);
            if (booking == null) return NotFound("Booking not found.");
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null) return BadRequest("Invalid booking data.");

            var booking = new Booking
            {   Id = bookingDto.Id,
                AccountId = bookingDto.AccountId,
                DiscountId = bookingDto.DiscountId,
                Description = bookingDto.Description,
                BookingDate = bookingDto.BookingDate,
                TotalPrice = bookingDto.TotalPrice,
                Status = bookingDto.Status,
                IsActive = true // Defaulting IsActive to true when creating a booking
            };

            await _bookingService.AddBooking(booking);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, bookingDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null || id != bookingDto.Id) return BadRequest("Invalid booking data.");

            var existingBooking = await _bookingService.GetBookingById(id);
            if (existingBooking == null) return NotFound("Booking not found.");

            var booking = new Booking
            {
                Id = bookingDto.Id,
                AccountId = bookingDto.AccountId,
                DiscountId = bookingDto.DiscountId,
                Description = bookingDto.Description,
                BookingDate = bookingDto.BookingDate,
                TotalPrice = bookingDto.TotalPrice,
                Status = bookingDto.Status,
                IsActive = true // Assuming it remains active when updating
            };

            await _bookingService.UpdateBooking(booking);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingBooking = await _bookingService.GetBookingById(id);
            if (existingBooking == null) return NotFound("Booking not found.");
            await _bookingService.DeleteBooking(id);
            return NoContent();
        }
    }
}
