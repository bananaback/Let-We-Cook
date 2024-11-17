using LetWeCook.Common.Results;
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

            var getProfileResult = await _profileService.GetUserProfileAsync(userIdString, cancellationToken);

            _logger.LogInformation($"{getProfileResult.Message}\n{getProfileResult.Data}\n{getProfileResult.Exception?.Message}");
            var profileDTO = getProfileResult.Data;


            if (!getProfileResult.IsSuccess)
            {
                return View("ProfileError");
            }

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

            var updateProfileResult = await _profileService.UpdateUserProfileAsync(profileDTO, cancellationToken);

            if (!updateProfileResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, updateProfileResult.Message);
                return View(model);
            }

            _logger.LogInformation("User profile updated successfully.");
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

            Result<List<RecipeDTO>> result = await _recipeService.GetAllRecipeOverviewByUserIdAsync(userId, cancellationToken);

            if (!result.IsSuccess)
            {
                // Log the error, or take appropriate action
                TempData["ErrorMessage"] = "There was an issue fetching your recipes. Please try again later.";

                return View("Error");
            }

            return View(result.Data);
        }

    }
}
