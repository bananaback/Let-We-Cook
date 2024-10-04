using Microsoft.AspNetCore.Mvc;

namespace LetWeCook.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class AuthController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
    }
}
