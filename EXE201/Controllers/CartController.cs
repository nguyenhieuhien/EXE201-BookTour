using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(long id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null) return NotFound();
            return Ok(cart);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetCartsByAccountId(long accountId)
        {
            var carts = await _cartService.GetCartsByAccountIdAsync(accountId);
            return Ok(carts);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateCart([FromBody] CartDTO cartDto)
        //{
        //    if (cartDto == null) return BadRequest("Invalid request.");

        //    var cart = new Cart
        //    {
        //        AccountId = cartDto.AccountId,
        //        IsActive = cartDto.IsActive
        //    };

        //    await _cartService.AddCartAsync(cart);
        //    return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, cart);
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateCart(long id, [FromBody] CartDTO cartDto)
        //{
        //    if (cartDto == null) return BadRequest("Invalid request.");

        //    var existingCart = await _cartService.GetCartByIdAsync(id);
        //    if (existingCart == null) return NotFound();

        //    existingCart.AccountId = cartDto.AccountId;
        //    existingCart.IsActive = cartDto.IsActive;

        //    await _cartService.UpdateCartAsync(existingCart);
        //    return NoContent();
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(long id)
        {
            var existingCart = await _cartService.GetCartByIdAsync(id);
            if (existingCart == null) return NotFound();

            await _cartService.DeleteCartAsync(id);
            return NoContent();
        }
    }
}
