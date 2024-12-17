using LetWeCook.Services.DTOs;
using LetWeCook.Services.RecipeServices;
using LetWeCook.Web.Areas.Cooking.Models.Requests;
using LetWeCook.Web.Areas.Cooking.Models.ViewModels;
using LetWeCook.Web.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LetWeCook.Web.Areas.Cooking.Controllers
{
    [Area("Cooking")]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        public async Task<IActionResult> Index(string searchTerm = "", string cuisine = "", string difficulty = "", int cookTime = 0, int servings = 0, string sortBy = "", int itemsPerPage = 10, int currentPage = 1, CancellationToken cancellationToken = default)
        {
            RecipeViewModel model = new RecipeViewModel
            {
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                SearchTerm = searchTerm,
                Cuisine = cuisine,
                Difficulty = difficulty,
                CookTime = cookTime,
                Servings = servings,
                SortBy = sortBy

            };

            var paginatedRecipes = await _recipeService.SearchRecipesAsync(searchTerm, cuisine, difficulty, cookTime, servings, sortBy, itemsPerPage, currentPage, cancellationToken);
            model.Recipes = paginatedRecipes.Items;
            model.TotalPages = (int)Math.Ceiling((double)paginatedRecipes.TotalItems / itemsPerPage);

            return View(model);
        }

        public async Task<IActionResult> UserRecipes(Guid id, CancellationToken cancellationToken = default)
        {

            List<RecipeDTO> recipes = await _recipeService.GetAllRecipeOverviewByUserIdAsync(id, cancellationToken);
            ViewData["UserId"] = id;

            return View(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
        {
            RecipeDTO recipeDTO = await _recipeService.GetRecipeByIdAsync(id, cancellationToken);

            return View(recipeDTO);

        }

        [HttpGet("/api/recipes/{id:guid}")]
        public async Task<IActionResult> RecipeDetails(Guid id, CancellationToken cancellationToken = default)
        {
            RecipeDTO recipeDTO = await _recipeService.GetRecipeByIdAsync(id, cancellationToken);
            return Ok(recipeDTO);
        }

        [HttpPost("api/recipes/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _recipeService.DeleteRecipeByIdAsync(id, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message));
            }
        }

        [HttpPut("/api/recipes/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] EditRecipeRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                // Log or return the model state errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();

                return BadRequest(new { Message = "Model binding failed", Errors = errors });
            }

            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            RecipeDTO recipeDTO = new RecipeDTO
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                Cuisine = request.Cuisine,
                Difficulty = request.Difficulty,
                CookTimeInMinutes = request.CookingTimeInMinutes,
                Serving = request.Serving,
                CreatedBy = userId,
                RecipeCoverImage = new MediaUrlDTO
                {
                    Id = Guid.Parse(request.CoverImageId)
                },
                DateCreated = DateTime.Now,
                RecipeIngredientDTOs = request.RecipeIngredientDTOs,
                StepDTOs = request.StepDTOs
            };

            var updatedRecipe = await _recipeService.UpdateRecipe(userIdString, recipeDTO, cancellationToken);

            return Ok(updatedRecipe);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // Log or return the model state errors for debugging
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();

                return BadRequest(new { Message = "Model binding failed", Errors = errors });
            }

            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            RecipeDTO recipeDTO = new RecipeDTO
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Cuisine = request.Cuisine,
                Difficulty = request.Difficulty,
                CookTimeInMinutes = request.CookingTimeInMinutes,
                Serving = request.Serving,
                CreatedBy = userId,
                RecipeCoverImage = new MediaUrlDTO
                {
                    Id = Guid.Parse(request.CoverImageId)
                },
                DateCreated = DateTime.Now,
                RecipeIngredientDTOs = request.RecipeIngredientDTOs,
                StepDTOs = request.StepDTOs
            };


            RecipeDTO createRecipeResult = await _recipeService.CreateRecipeAsync(userIdString, recipeDTO, cancellationToken);

            return Ok(new
            {
                Message = "Recipe created successfully",
                Recipe = createRecipeResult
            });
        }

    }
}
