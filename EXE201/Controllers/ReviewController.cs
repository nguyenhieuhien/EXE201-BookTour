using EXE201.Models;
using EXE201.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using EXE201.Controllers.DTO.Review;
using EXE201.Service;

namespace EXE201.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IAccountService _accountService;
        private readonly IPackageService _packageService;

        public ReviewController(IReviewService reviewService, IAccountService accountService, IPackageService packageService)
        {
            _reviewService = reviewService;
            _accountService = accountService;
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAll()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            var result = reviews.Select(review => new ReviewDTO
            {
                Id = review.Id,
                AccountId = review.AccountId,
                PackageId = review.PackageId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreateDate = (DateTime)review.CreateDate,
                IsActive = review.IsActive,
            }).ToList();

            return Ok(result);
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Review>>> GetAll()
        //{
        //    var reviews = await _reviewService.GetAllReviewsAsync();
        //    return Ok(reviews);
        //}



        [HttpGet("{id}")]
        public async Task<ActionResult<ReviewDTO>> GetById(long id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound(new { Message = $"Review with ID {id} was not found." });

            return Ok(new ReviewDTO
            {
                Id = review.Id,
                AccountId = review.AccountId,
                PackageId = review.PackageId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreateDate = (DateTime)review.CreateDate,
                IsActive = review.IsActive,
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(ReviewDTO reviewDTO)
        {
            var existingAccount = await _accountService.GetByIdAsync(reviewDTO.AccountId);
            var existingPackage = await _packageService.GetPackageByIdAsync(reviewDTO.PackageId);
            if (existingAccount == null)
            {
                return NotFound(new { Message = $"Account with ID {reviewDTO.AccountId} was not found." });
            }
            if (existingPackage == null)
            {
                return NotFound(new { Message = $"Package with ID {reviewDTO.PackageId} was not found." });
            }
            if (reviewDTO.Rating < 1 || reviewDTO.Rating > 5)
            {
                return BadRequest(new { Message = "Rating must be between 1 and 5." });
            }

            var review = new Review
            {
                Id = reviewDTO.Id,
                AccountId = reviewDTO.AccountId,
                PackageId = reviewDTO.PackageId,
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                CreateDate = (DateTime)reviewDTO.CreateDate, // Chuyển từ DateTime sang DateOnly
                IsActive = reviewDTO.IsActive,
            };

            await _reviewService.AddReviewAsync(review);

            return CreatedAtAction(nameof(GetById), new { id = review.Id }, new
            {
                Message = "Review created successfully.",
                Data = reviewDTO
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, ReviewDTOUpdate reviewDTOUpdate)
        {

            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
            {
                return NotFound(new { Message = $"No Review found with ID {id}." });
            }

            if (reviewDTOUpdate.Rating < 1 || reviewDTOUpdate.Rating > 5)
            {
                return BadRequest(new { Message = "Rating must be between 1 and 5." });
            }

            existingReview.Rating = reviewDTOUpdate.Rating;
            existingReview.Comment = reviewDTOUpdate.Comment;
            await _reviewService.UpdateReviewAsync(existingReview);

            return Ok(new
            {
                Message = "Review updated successfully.",
                Data = new
                {
                    Id = existingReview.Id,
                    AccountId = existingReview.AccountId,
                    PackageId = existingReview.PackageId,
                    Rating = existingReview.Rating,
                    Comment = existingReview.Comment,
                    CreateDate = existingReview.CreateDate,
                    IsActive = existingReview.IsActive,
                }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)

                return NotFound(new { Message = $"No Review found with ID {id}." });
            await _reviewService.DeleteReviewAsync(id);
            return Ok(new
            {
                Message = $"Review with ID {id} has been deleted successfully.",
                Data = new
                {
                    Id = existingReview.Id,
                    AccountId = existingReview.AccountId,
                    PackageId = existingReview.PackageId,
                    Rating = existingReview.Rating,
                    Comment = existingReview.Comment,
                    CreateDate = existingReview.CreateDate,
                    IsActive = existingReview.IsActive,
                }
            });
        }
    }
}
