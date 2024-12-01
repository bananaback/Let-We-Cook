using LetWeCook.Services.DTOs;
using LetWeCook.Services.ProfileServices;
using LetWeCook.Services.RecipeServices;
using LetWeCook.Web.Areas.Account.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LetWeCook.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class ProfileController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(IRecipeService recipeService, IProfileService profileService, ILogger<ProfileController> logger)
        {
            _recipeService = recipeService;
            _profileService = profileService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var profileDTO = await _profileService.GetUserProfileAsync(userIdString, cancellationToken);

            return View(new ProfileViewModel
            {
                Username = profileDTO!.UserName,
                Email = profileDTO.Email,
                DateJoined = profileDTO.DateJoined,
                PhoneNumber = profileDTO.PhoneNumber,
                FirstName = profileDTO.FirstName,
                LastName = profileDTO.LastName,
                Age = profileDTO.Age,
                Gender = profileDTO.Gender,
                Address = profileDTO.Address,
            });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            Guid userIdGuid;
            bool isValid = Guid.TryParse(userIdString, out userIdGuid);

            if (!isValid)
            {
                return View("ProfileError");
            }


            var profileDTO = new ProfileDTO
            {
                UserId = userIdGuid,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                Gender = model.Gender,
                Address = model.Address,
            };

            var updatedProfile = await _profileService.UpdateUserProfileAsync(profileDTO, cancellationToken);

            ViewData["SuccessMessage"] = "Profile updated successfully!";
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Recipes(CancellationToken cancellationToken = default)
        {
            string userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized();
            }

            List<RecipeDTO> recipes = await _recipeService.GetAllRecipeOverviewByUserIdAsync(userId, cancellationToken);

            return View(recipes);
        }

    }
}
