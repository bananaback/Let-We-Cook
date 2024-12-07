using LetWeCook.Services.DTOs;
using LetWeCook.Services.RecipeServices;
using LetWeCook.Web.Areas.Cooking.Models.Requests;
using LetWeCook.Web.Areas.Cooking.Models.ViewModels;
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

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
        {
            RecipeDTO recipeDTO = await _recipeService.GetRecipeByIdAsync(id, cancellationToken);

            return View(recipeDTO);

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
