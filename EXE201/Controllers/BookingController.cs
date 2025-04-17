
using EXE201.Controllers.DTO.Booking;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Chua sua
    /// </summary>
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
        public async Task<IActionResult> Create([FromBody] BookingDTOCreate bookingDtoCreate)
        {
            //if (bookingDtoCreate == null) return BadRequest("Invalid booking data.");

            var booking = new Booking
            {   
                //Id = bookingDto.Id,
                AccountId = bookingDtoCreate.AccountId,
                DiscountId = bookingDtoCreate.DiscountId,
                Description = bookingDtoCreate.Description,
                BookingDate = bookingDtoCreate.BookingDate,
                TotalPrice = 0,
                Status = bookingDtoCreate.Status,
                IsActive = true 
            };

            await _bookingService.AddBooking(booking);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, bookingDtoCreate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] BookingDTOUpdate bookingDtoUpdate)
        {
            //if (bookingDtoUpdate == null) return BadRequest("Invalid booking data.");

            var existingBooking = await _bookingService.GetBookingById(id);
            if (existingBooking == null) return NotFound("Booking not found.");

            var booking = new Booking
            {
                //Id = bookingDto.Id,
                Description = bookingDtoUpdate.Description,
                BookingDate = bookingDtoUpdate.BookingDate,
                TotalPrice = bookingDtoUpdate.TotalPrice,
                Status = bookingDtoUpdate.Status,
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
