using LetWeCook.Services.DishCollectionServices;
using LetWeCook.Services.DTOs;
using LetWeCook.Web.Areas.Account.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LetWeCook.Web.Areas.Account.Controllers
{
    public class DishCollectionController : Controller
    {
        private readonly IDishCollectionService _dishCollectionService;
        public DishCollectionController(IDishCollectionService dishCollectionService)
        {
            _dishCollectionService = dishCollectionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost("/api/collections")] // Handle POST requests to api/collections
        public async Task<IActionResult> Create([FromBody] CreateDishCollectionRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return 400 with model validation errors
            }

            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            Guid userIdGuid;
            bool isValid = Guid.TryParse(userIdString, out userIdGuid);

            if (!isValid)
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            var dishCollectionDTO = new DishCollectionDTO
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                DateCreated = DateTime.Now,
            };


            try
            {
                var savedDishCollectionDTO = await _dishCollectionService.CreateDishCollectionAsync(userIdString, dishCollectionDTO, cancellationToken);

                return CreatedAtAction(nameof(Create), new { id = savedDishCollectionDTO.Id }, savedDishCollectionDTO); // Return 201 with the created collection
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("/api/collections")]
        public async Task<IActionResult> GetUserCollections(CancellationToken cancellation = default)
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            Guid userIdGuid;
            bool isValid = Guid.TryParse(userIdString, out userIdGuid);

            if (!isValid)
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            var collections = await _dishCollectionService.GetUserDishCollectionsAsync(userIdString, cancellation);

            return Ok(collections); // Return 200 with the list of collections

        }

        [Authorize]
        [HttpPost("/api/collections/{collectionId}/add-recipe/{recipeId}")]
        public async Task<IActionResult> AddRecipeToCollection(Guid collectionId, Guid recipeId, CancellationToken cancellationToken = default)
        {
            // Extract the user ID from the authentication context
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            Guid userIdGuid;
            bool isValid = Guid.TryParse(userIdString, out userIdGuid);

            if (!isValid)
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            try
            {
                // Call the service to add the recipe to the collection
                await _dishCollectionService.AddRecipeToCollectionAsync(userIdString, collectionId, recipeId, cancellationToken);

                return Ok(new { message = "Recipe added to collection successfully." }); // Return 200 on success
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // Return 404 if the collection or recipe is not found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 for other errors
            }
        }

        [Authorize]
        [HttpDelete("/api/collections/{collectionId}")]
        public async Task<IActionResult> Delete(Guid collectionId, CancellationToken cancellationToken = default)
        {
            // Extract the user ID from the authentication context
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            Guid userIdGuid;
            bool isValid = Guid.TryParse(userIdString, out userIdGuid);

            if (!isValid)
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            try
            {
                // Call the service to delete the collection
                bool isDeleted = await _dishCollectionService.DeleteDishCollectionAsync(userIdString, collectionId, cancellationToken);

                if (!isDeleted)
                {
                    return NotFound("Dish collection not found or not owned by the user"); // Return 404 if the collection is not found or doesn't belong to the user
                }

                return NoContent(); // Return 204 for successful deletion without content
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 for any unexpected errors
            }
        }

        [HttpDelete("/api/collections/{collectionId}/remove-recipe/{recipeId}")]
        public async Task<IActionResult> RemoveRecipeFromCollectionAsync(Guid collectionId, Guid recipeId, CancellationToken cancellationToken)
        {
            // Extract the user ID from the authentication context
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            Guid userIdGuid;
            bool isValid = Guid.TryParse(userIdString, out userIdGuid);

            if (!isValid)
            {
                return Unauthorized("Invalid user ID"); // Return 401 if user ID is invalid
            }

            try
            {
                // Call the service to remove the recipe from the collection
                var deleteResult = await _dishCollectionService.RemoveRecipeFromCollection(userIdString, collectionId, recipeId, cancellationToken);

                if (deleteResult)
                {
                    // Return success response
                    return Ok(new { message = "Recipe removed from collection successfully." });
                }
                else
                {
                    // Handle case where removal failed
                    return BadRequest(new { message = "Failed to remove recipe from collection. Please try again." });
                }
            }
            catch (KeyNotFoundException ex)
            {
                // Return 404 if the collection or recipe is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Return 500 for other errors
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }


    }
}
