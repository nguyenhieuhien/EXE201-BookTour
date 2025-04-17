using EXE201.Controllers.DTO.BookingDetail;
using EXE201.Controllers.DTO.Service;
using EXE201.Models;
using EXE201.Service;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingDetailController : ControllerBase
    {
        private readonly IBookingDetailService _bookingDetailService;

        public BookingDetailController(IBookingDetailService bookingDetailService)
        {
            _bookingDetailService = bookingDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDetailDTO>>> GetAll()
        {
            var bookingDetails = await _bookingDetailService.GetAllBookingDetails();
            var result = new List<BookingDetailDTO>();
            foreach (var bookingDetail in bookingDetails)
            {
                result.Add(new BookingDetailDTO
                {
                    Id = bookingDetail.Id,
                    BookingId = bookingDetail.BookingId,
                    PackageId = bookingDetail.PackageId,
                    IsActive = bookingDetail.IsActive
                });
            }
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDetailDTO>> GetById(long id)
        {
            var bookingDetail = await _bookingDetailService.GetBookingDetailById(id);
            if (bookingDetail == null)
                return NotFound(new { Message = $"BookingDetail with ID {id} was not found." });

            return Ok(new BookingDetailDTO
            {
                Id = bookingDetail.Id,
                BookingId = bookingDetail.BookingId,
                PackageId = bookingDetail.PackageId,
                IsActive = bookingDetail.IsActive
            });
        }


        [HttpPost]
        public async Task<IActionResult> Create(BookingDetailDTOCreate bookingDetailDtoCreate)
        {
            //if (bookingDetailDtoCreate == null) 
            //    return BadRequest("Invalid booking detail data.");

            var bookingDetail = new BookingDetail
             { 
                //Id = bookingDetailDto.Id,
                BookingId = bookingDetailDtoCreate.BookingId,
                PackageId = bookingDetailDtoCreate.PackageId,
                IsActive = true,
            };

            await _bookingDetailService.AddBookingDetail(bookingDetail);
            return CreatedAtAction(nameof(GetById), new { id = bookingDetail.Id }, new
            {
                Message = "BookingDetail created successfully.",
                Data = bookingDetailDtoCreate
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, BookingDetailDTOUpdate bookingDetailDtoUpdate)
        {
            //if (bookingDetailDtoUpdate == null) return BadRequest("Invalid booking detail data.");

            var existingBookingDetail = await _bookingDetailService.GetBookingDetailById(id);
            if (existingBookingDetail == null)
                return NotFound(new { Message = $"No BookingDetail found with ID {id}." });

            // Cập nhật trực tiếp thuộc tính của thực thể đang theo dõi
            existingBookingDetail.BookingId = bookingDetailDtoUpdate.BookingId;
            existingBookingDetail.PackageId = bookingDetailDtoUpdate.PackageId;
            //existingBookingDetail.IsActive = bookingDetailDto.IsActive;

            await _bookingDetailService.UpdateBookingDetail(existingBookingDetail);
            return Ok(new
            {
                Message = "BookingDetail updated successfully.",
                Data = new
                {
                    Id = existingBookingDetail.Id,
                    BookingId = existingBookingDetail.BookingId,
                    PackageId = existingBookingDetail.PackageId,
                    IsActive = existingBookingDetail.IsActive
                }
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingBookingDetail = await _bookingDetailService.GetBookingDetailById(id);
            if (existingBookingDetail == null) 
                return NotFound(new { Message = $"No BookingDetail found with ID {id}." });

            await _bookingDetailService.DeleteBookingDetail(id);
            return Ok(new
            {
                Message = $"BookingDetail with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingBookingDetail.Id,
                    BookingId = existingBookingDetail.BookingId,
                    PackageId = existingBookingDetail.PackageId,
                    IsActive = existingBookingDetail.IsActive
                }
            });
        }
    }
}
