using EXE201.Controllers.DTO.Discount;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetAll()
        {
            var discounts = await _discountService.GetAllDiscountsAsync();
            var result = new List<DiscountDTO>();
            foreach (var discount in discounts)
            {
                result.Add(new DiscountDTO
                {
                    Id = discount.Id,
                    Code = discount.Code,
                    Percentage = discount.Percentage,
                    ExpiryDate = discount.ExpiryDate,
                    IsActive = discount.IsActive,
                });
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDTO>> GetById(long id)
        {
            var discount = await _discountService.GetDiscountByIdAsync(id);
            if (discount == null)
                return NotFound(new { Message = $"Discount with ID {id} was not found." });

            return Ok(new DiscountDTO
            {
                Id = discount.Id,
                Code = discount.Code,
                Percentage = discount.Percentage,
                ExpiryDate = discount.ExpiryDate,
                IsActive = discount.IsActive,
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(DiscountDTO discountDTO)
        {
            var discount = new Discount
            {
                Id = discountDTO.Id,
                Code = discountDTO.Code,
                Percentage = discountDTO.Percentage,
                ExpiryDate = discountDTO.ExpiryDate,
                IsActive = discountDTO.IsActive,
            };

            await _discountService.AddDiscountAsync(discount);

            return CreatedAtAction(nameof(GetById), new { id = discount.Id }, new
            {
                Message = "Discount created successfully.",
                Data = discountDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, DiscountDTOUpdate discountDTOUpdate)
        {
            var existingDiscount = await _discountService.GetDiscountByIdAsync(id);
            if (existingDiscount == null)
            {
                return NotFound(new { Message = $"No Discount found with ID {id}." });
            }

            existingDiscount.Code = discountDTOUpdate.Code;
            existingDiscount.Percentage = discountDTOUpdate.Percentage;
            existingDiscount.ExpiryDate = discountDTOUpdate.ExpiryDate;

            await _discountService.UpdateDiscountAsync(existingDiscount);

            return Ok(new
            {
                Message = "Discount updated successfully.",
                Data = new
                {
                    Id = existingDiscount.Id,
                    Code = existingDiscount.Code,
                    Percentage = existingDiscount.Percentage,
                    ExpiryDate = existingDiscount.ExpiryDate,
                    IsActive = existingDiscount.IsActive,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingDiscount = await _discountService.GetDiscountByIdAsync(id);
            if (existingDiscount == null)
            {
                return NotFound(new { Message = $"No Discount found with ID {id}." });
            }

            await _discountService.DeleteDiscountAsync(id);
            return Ok(new
            {
                Message = $"Discount with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingDiscount.Id,
                    Code = existingDiscount.Code,
                    Percentage = existingDiscount.Percentage,
                    ExpiryDate = existingDiscount.ExpiryDate,
                    IsActive = existingDiscount.IsActive,
                }
            });
        }
    }
}
