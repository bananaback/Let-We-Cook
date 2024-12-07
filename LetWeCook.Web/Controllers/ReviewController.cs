using LetWeCook.Services.RecipeReviewServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LetWeCook.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IRecipeReviewService _recipeReviewService;
        public ReviewController(IRecipeReviewService recipeReviewService)
        {
            _recipeReviewService = recipeReviewService;

        }

        [Authorize]
        [HttpPost("/api/reviews")]
        public async Task<IActionResult> CreateReview(
            [FromQuery] string review,
            [FromQuery] decimal rating,
            [FromQuery] Guid recipeId,
            CancellationToken cancellationToken = default)
        {
            // Get the user's ID from the claims
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (!Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            // Validate input
            if (string.IsNullOrWhiteSpace(review))
            {
                return BadRequest("Review text cannot be empty."); // Return 400 for invalid input
            }

            if (rating < 0 || rating > 5)
            {
                return BadRequest("Rating must be between 0 and 5."); // Return 400 for invalid rating
            }

            try
            {
                // Call the service to create the review
                var reviewDTO = await _recipeReviewService.CreateReviewForUser(userIdString, recipeId, review, rating, cancellationToken);

                // Return a success response
                return CreatedAtAction(nameof(CreateReview), new { id = reviewDTO.Id }, reviewDTO);
            }
            catch (ArgumentException ex)
            {
                // Handle validation issues or other errors thrown by the service
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                // Example: _logger.LogError(ex, "An error occurred while creating the review");
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [Authorize]
        [HttpDelete("/api/reviews/{reviewId}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId, CancellationToken cancellationToken = default)
        {
            // Get the user's ID from the claims
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            if (!Guid.TryParse(userIdString, out Guid userIdGuid))
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            // Call the service to delete the review
            var deleted = await _recipeReviewService.DeleteReviewForUser(userIdString, reviewId, cancellationToken);

            if (!deleted)
            {
                return NotFound("Review not found or you do not have permission to delete it.");
            }

            // Return a success response
            return Ok();


        }

    }
}
