using EXE201.Controllers.DTO.Cart;
using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;

        public CartController(ICartService cartService,IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }


     



        [HttpGet("{id}")]
        public async Task<ActionResult<CartDTO>> GetCartById(long id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            if (cart == null)
                return NotFound(new { Message = $"Cart with ID {id} was not found." });

            return Ok(new CartDTO
            {
                Id = cart.Id,
                AccountId = cart.AccountId,
                IsActive = cart.IsActive,
            });
        }

        //[HttpGet("{accountId}")]
        //public async Task<ActionResult<CartDTO>> GetCartByAccountId(long accountId)
        //{
        //    var cart = await _cartService.GetCartsByAccountIdAsync(accountId);
        //    if (cart == null)
        //        return NotFound(new { Message = $"Cart with account ID {accountId} was not found." });

        //    return Ok(new CartDTO
        //    {
        //        Id = cart.Id,
        //        AccountId = cart.AccountId,
        //        IsActive = cart.IsActive,
        //    });
        //}



        //[HttpPost]
        //public async Task<ActionResult> Create(CartDTOCreate cartDTOCreate)
        //{
        //    var existingAccount = await _accountService.GetByIdAsync(cartDTOCreate.AccountId);
        //    if (existingAccount == null)
        //    {
        //        return NotFound(new { Message = $"Account with ID {cartDTOCreate.AccountId} was not found." });
        //    }

        //    var cart = new Cart
        //    {
        //      //Id= cartDTO.Id,
        //      AccountId = cartDTOCreate.AccountId,
        //      IsActive = true,
        //    };

        //    await _cartService.AddCartAsync(cart);

        //    return CreatedAtAction(nameof(GetCartById), new { id = cart.Id }, new
        //    {
        //        Message = "Cart created successfully.",
        //        Data = cartDTOCreate
        //    });
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult> Update(long id, ReviewDTOUpdate reviewDTOUpdate)
        //{

        //    var existingReview = await _reviewService.GetReviewByIdAsync(id);
        //    if (existingReview == null)
        //    {
        //        return NotFound(new { Message = $"No Review found with ID {id}." });
        //    }

        //    if (reviewDTOUpdate.Rating < 1 || reviewDTOUpdate.Rating > 5)
        //    {
        //        return BadRequest(new { Message = "Rating must be between 1 and 5." });
        //    }

        //    existingReview.Rating = reviewDTOUpdate.Rating;
        //    existingReview.Comment = reviewDTOUpdate.Comment;
        //    await _reviewService.UpdateReviewAsync(existingReview);

        //    return Ok(new
        //    {
        //        Message = "Review updated successfully.",
        //        Data = new
        //        {
        //            Id = existingReview.Id,
        //            AccountId = existingReview.AccountId,
        //            PackageId = existingReview.PackageId,
        //            Rating = existingReview.Rating,
        //            Comment = existingReview.Comment,
        //            CreateDate = existingReview.CreateDate,
        //            IsActive = existingReview.IsActive,
        //        }
        //    });
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(long id)
        //{
        //    var existingCart = await _cartService.GetCartByIdAsync(id);
        //    if (existingCart == null)

        //        return NotFound(new { Message = $"No Cart found with ID {id}." });
        //    await _cartService.DeleteCartAsync(id);
        //    return Ok(new
        //    {
        //        Message = $"Cart with ID {id} has been deleted successfully.",
        //        Data = new
        //        {
        //           Id = existingCart.Id,
        //           AccountId =existingCart.AccountId,
        //           IsActive = existingCart.IsActive,
        //        }
        //    });
        //}
    }
}
