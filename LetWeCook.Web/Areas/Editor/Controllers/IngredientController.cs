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
            throw new NotImplementedException();
        }

    }
}
