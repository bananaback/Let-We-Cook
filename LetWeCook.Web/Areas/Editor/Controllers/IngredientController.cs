using LetWeCook.Services.DTOs;
using LetWeCook.Services.IngredientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.Areas.Editor.Controllers
{
    [Area("Editor")]
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        // Secured Index action for Admin role only
        [Authorize(Roles = "Admin")] // Only users with the Admin role can access this action
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // Secured Index action for Admin role only
        [Authorize(Roles = "Admin")] // Only users with the Admin role can access this action
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken = default)
        {
            ViewData["IngredientId"] = id;
            return View();
        }

        // New endpoint that returns a single line of text
        [HttpGet]
        [AllowAnonymous] // Allow anyone to access this action
        public IActionResult TestEndpoint()
        {
            return Content("This is a test response from the TestEndpoint."); // Returns a simple line of text
        }

        [HttpGet]
        public async Task<IActionResult> GetIngredientList(CancellationToken cancellationToken)
        {
            // Call service to get the ingredient names and ids
            List<IngredientDTO> result = await _ingredientService.GetIngredientsNameAndIdAsync(cancellationToken);

            // Return the result with success status and list of IngredientDTOs
            return Json(new { isSuccess = true, data = result });
        }

    }
}
