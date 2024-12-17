using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.Areas.Editor.Controllers
{
    [Area("Editor")]
    public class RecipeController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Authorize]

        public async Task<IActionResult> Edit(Guid id)
        {
            ViewData["RecipeId"] = id;

            return View();
        }
    }
}
