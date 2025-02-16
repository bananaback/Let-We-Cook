using LetWeCook.Services.DishCollectionServices;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.ProfileServices;
using LetWeCook.Services.RecipeServices;
using LetWeCook.Services.UserDietaryPreferenceServices;
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
        private readonly IDishCollectionService _dishCollectionService;
        private readonly IUserDietaryPreferenceService _userDietaryPreferenceService;
        private readonly ILogger<ProfileController> _logger;
        public ProfileController(IRecipeService recipeService, IProfileService profileService, IDishCollectionService dishCollectionService, IUserDietaryPreferenceService userDietaryPreferenceService, ILogger<ProfileController> logger)
        {
            _recipeService = recipeService;
            _profileService = profileService;
            _dishCollectionService = dishCollectionService;
            _userDietaryPreferenceService = userDietaryPreferenceService;
            _logger = logger;
        }

        [HttpGet("/api/profile/{id:guid}")]
        public async Task<IActionResult> GetUserBio(Guid id, CancellationToken cancellationToken = default)
        {
            return Ok(await _profileService.GetUserProfileAsync(id.ToString(), cancellationToken));
        }

        [HttpGet]
        public async Task<IActionResult> DietaryPreference(CancellationToken cancellationToken = default)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException("User not authenticated."));

            var dietaryPreferences = await _userDietaryPreferenceService.GetUserDietaryPreferencesAsync(userId, cancellationToken);

            var viewModel = new UserDietaryPreferencesViewModel
            {
                Preferences = dietaryPreferences.Select(dp => new DietaryPreferenceViewModel
                {
                    Name = dp.Value,
                    Description = dp.Description,
                    Color = dp.Color,
                    Icon = dp.Icon,
                    IsSelected = dp.IsSelected
                }).ToList()
            };

            return View(viewModel);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveDietaryPreferences(UserDietaryPreferencesViewModel model, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
            {
                _logger.LogWarning("User ID not found or invalid.");
                return Unauthorized();
            }

            _logger.LogInformation("Saving dietary preferences for User ID: {UserId}", parsedUserId);

            if (model.Preferences == null || model.Preferences.Count == 0)
            {
                _logger.LogWarning("No dietary preferences received.");
                return View("DietaryPreference", model);
            }

            var saveDto = new SaveDietaryPreferencesDTO
            {
                Preferences = model.Preferences.Select(p => new DietaryPreferenceDTO
                {
                    Value = p.Name,
                    Description = p.Description,
                    Color = p.Color,
                    Icon = p.Icon,
                    IsSelected = p.IsSelected
                }).ToList()
            };

            await _userDietaryPreferenceService.SaveDietaryPreferencesAsync(parsedUserId, saveDto, cancellationToken);

            _logger.LogInformation("Dietary preferences saved successfully.");

            // Store success message in TempData
            TempData["SuccessMessage"] = "Dietary preferences saved successfully!";

            // Redirect to prevent form resubmission
            return RedirectToAction("DietaryPreference");
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

        [Authorize]
        public async Task<IActionResult> RecipeCollections(CancellationToken cancellationToken = default)
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CollectionDetails(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                // Fetch the collection details from the service
                var dishCollectionDTO = await _dishCollectionService.GetCollectionDetailsByIdAsync(id, cancellationToken);

                if (dishCollectionDTO == null)
                {
                    return NotFound("Collection not found."); // Return 404 if the collection does not exist
                }

                // Map the DTO to the ViewModel
                var viewModel = new CollectionDetailsViewModel
                {
                    CollectionId = dishCollectionDTO.Id,
                    CollectionName = dishCollectionDTO.Name,
                    CollectionDescription = dishCollectionDTO.Description,
                    Recipes = dishCollectionDTO.Recipes
                };

                // Pass the ViewModel to the view
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log the error (optional) and return an error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
