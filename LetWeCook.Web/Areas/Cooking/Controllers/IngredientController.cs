using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.IngredientServices;
using LetWeCook.Web.Areas.Cooking.Models.Requests;
using LetWeCook.Web.Areas.Cooking.Models.ViewModels;
using LetWeCook.Web.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.Areas.Cooking.Controllers
{
    [Area("Cooking")]
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientController> _logger;


        public IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search = "", int page = 1, int itemsPerPage = 10, CancellationToken cancellationToken = default)
        {
            var model = new IngredientViewModel
            {
                SearchTerm = search,
                CurrentPage = page,
                ItemsPerPage = itemsPerPage
            };

            var paginatedRecipes = await _ingredientService.SearchIngredientsAsync(search, page, itemsPerPage, cancellationToken);


            model.Ingredients = paginatedRecipes.Items;
            model.TotalPages = (int)Math.Ceiling((double)paginatedRecipes.TotalItems / itemsPerPage);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateIngredient([FromBody] CreateIngredientRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join("; ", ModelState
                    .Where(ms => ms.Value!.Errors.Count > 0)
                    .SelectMany(ms => ms.Value!.Errors.Select(e => e.ErrorMessage)));

                return BadRequest(new ErrorResponse(errorMessage));
            }

            RawIngredientDTO rawIngredientDto = new RawIngredientDTO
            {
                IngredientName = request.IngredientName,
                IngredientDescription = request.IngredientDescription,
                CoverImageUrl = request.CoverImageUrl,
                RawFrameDTOs = request.RawFrameDTOs
            };

            try
            {
                IngredientDTO ingredientDTO = await _ingredientService.CreateIngredientAsync(rawIngredientDto, cancellationToken);
                return Ok(new
                {
                    Message = "Ingredient created successfully",
                    Ingredient = ingredientDTO
                });
            }
            catch (IngredientCreationException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
        {
            IngredientDTO ingredient = await _ingredientService.GetIngredientByIdAsync(id, cancellationToken);

            var ingredientDetailsViewModel = new IngredientDetailsViewModel
            {
                Id = ingredient.Id,
                IngredientName = ingredient.IngredientName,
                IngredientDescription = ingredient.IngredientDescription,
                CoverImageUrl = ingredient.CoverImageUrl,
                Frames = ingredient.Frames
            };

            return View(ingredientDetailsViewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Error()
        {
            return View();
        }
    }
}
