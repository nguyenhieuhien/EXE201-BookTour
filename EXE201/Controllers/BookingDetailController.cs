﻿using EXE201.Controllers.DTO.BookingDetail;
using EXE201.Models;
using EXE201.Service;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/booking-details")]
    public class BookingDetailController : ControllerBase
    {
        private readonly IBookingDetailService _bookingDetailService;

        public BookingDetailController(IBookingDetailService bookingDetailService)
        {
            _bookingDetailService = bookingDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookingDetails = await _bookingDetailService.GetAllBookingDetails();
            var bookingDetailDTOs = bookingDetails.Select(b => new BookingDetailDTO
            {
                Id = b.Id,
                BookingId = b.BookingId,
                PackageId = b.PackageId,
                IsActive = b.IsActive
            }).ToList();

            return Ok(bookingDetailDTOs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var bookingDetail = await _bookingDetailService.GetBookingDetailById(id);
            if (bookingDetail == null) return NotFound("Booking detail not found.");

            var bookingDetailDTO = new BookingDetailDTO
            {
                Id = bookingDetail.Id,
                BookingId = bookingDetail.BookingId,
                PackageId = bookingDetail.PackageId,
                IsActive = bookingDetail.IsActive
            };

            return Ok(bookingDetailDTO);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingDetailDTOCreate bookingDetailDtoCreate)
        {
            if (bookingDetailDtoCreate == null) return BadRequest("Invalid booking detail data.");

            var bookingDetail = new BookingDetail
             { 
                //Id = bookingDetailDto.Id,
                BookingId = bookingDetailDtoCreate.BookingId,
                PackageId = bookingDetailDtoCreate.PackageId,
                IsActive = true,
            };

            await _bookingDetailService.AddBookingDetail(bookingDetail);
            return CreatedAtAction(nameof(GetById), new { id = bookingDetail.Id }, bookingDetailDtoCreate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] BookingDetailDTOUpdate bookingDetailDtoUpdate)
        {
            if (bookingDetailDtoUpdate == null) return BadRequest("Invalid booking detail data.");

            var existingBookingDetail = await _bookingDetailService.GetBookingDetailById(id);
            if (existingBookingDetail == null) return NotFound("Booking detail not found.");

            // Cập nhật trực tiếp thuộc tính của thực thể đang theo dõi
            existingBookingDetail.BookingId = bookingDetailDtoUpdate.BookingId;
            existingBookingDetail.PackageId = bookingDetailDtoUpdate.PackageId;
            //existingBookingDetail.IsActive = bookingDetailDto.IsActive;

            await _bookingDetailService.UpdateBookingDetail(existingBookingDetail);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingBookingDetail = await _bookingDetailService.GetBookingDetailById(id);
            if (existingBookingDetail == null) return NotFound("Booking detail not found.");

            await _bookingDetailService.DeleteBookingDetail(id);
            return NoContent();
        }
    }
}
