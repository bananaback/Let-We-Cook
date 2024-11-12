using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;
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

            var result = await _ingredientService.SearchIngredientsAsync(search, page, itemsPerPage, cancellationToken);

            if (result.IsSuccess && result.Data != null)
            {
                model.Ingredients = result.Data.Items;
                model.TotalPages = (int)Math.Ceiling((double)result.Data.TotalItems / itemsPerPage);
            }
            else
            {
                model.TotalPages = 1;
            }

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
                CoverImageBase64 = request.CoverImageBase64,
                RawFrameDTOs = request.RawFrameDTOs
            };

            Result<IngredientDTO> result = await _ingredientService.CreateIngredientAsync(rawIngredientDto, cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = "Ingredient created successfully",
                    Ingredient = result.Data
                });
            }
            else
            {
                return BadRequest(new ErrorResponse($"{result.Message} {result.Exception}"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
        {
            Result<IngredientDTO> result = await _ingredientService.GetIngredientByIdAsync(id, cancellationToken);

            if (!result.IsSuccess)
            {
                return View("Error", result.Message);
            }

            if (result.Data == null)
            {
                return View("Error", "Ingredient with the specified ID was not found.");
            }

            var ingredientDetailsViewModel = new IngredientDetailsViewModel
            {
                Id = result.Data.Id,
                IngredientName = result.Data.IngredientName,
                IngredientDescription = result.Data.IngredientDescription,
                CoverImageUrl = result.Data.CoverImageUrl,
                Frames = result.Data.Frames
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
