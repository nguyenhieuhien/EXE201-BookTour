
using EXE201.Controllers.DTO.CartItem;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace EXE201.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        // Lấy CartItem theo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemDTO>> GetById(long id)
        {
            var cartItem = await _cartItemService.GetByIdAsync(id);
            if (cartItem == null) return NotFound();

            return Ok(new CartItemDTO
            {
                Id = cartItem.Id,
                PackageId = cartItem.PackageId,
                CartId = cartItem.CartId,
                IsActive = cartItem.IsActive
            });
        }

        // Lấy danh sách CartItem theo CartId
        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItemDTO>>> GetByCartId(long cartId)
        {
            var cartItems = await _cartItemService.GetByCartIdAsync(cartId);
            var result = new List<CartItemDTO>();

            foreach (var item in cartItems)
            {
                result.Add(new CartItemDTO
                {
                    Id = item.Id,
                    PackageId = item.PackageId,
                    CartId = item.CartId,
                    IsActive = item.IsActive
                });
            }

            return Ok(result);
        }

        // Thêm CartItem
        [HttpPost]
        public async Task<ActionResult> Add(CartItemDTO cartItemDTO)
        {
            var cartItem = new CartItem
            {   Id = cartItemDTO.Id,
                PackageId = cartItemDTO.PackageId,
                CartId = cartItemDTO.CartId,
                IsActive = cartItemDTO.IsActive
            };

            await _cartItemService.AddAsync(cartItem);
            return CreatedAtAction(nameof(GetById), new { id = cartItem.Id }, cartItemDTO);
        }

        // Cập nhật CartItem
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, CartItemDTO cartItemDTO)
        {
            var existingCartItem = await _cartItemService.GetByIdAsync(id);
            if (existingCartItem == null) return NotFound();

            existingCartItem.PackageId = cartItemDTO.PackageId;
            existingCartItem.CartId = cartItemDTO.CartId;
            existingCartItem.IsActive = cartItemDTO.IsActive;

            await _cartItemService.UpdateAsync(existingCartItem);
            return NoContent();
        }

        // Xóa CartItem
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var existingCartItem = await _cartItemService.GetByIdAsync(id);
            if (existingCartItem == null) return NotFound();

            await _cartItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
