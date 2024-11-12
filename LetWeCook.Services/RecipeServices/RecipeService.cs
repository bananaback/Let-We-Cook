using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMediaUrlRepository _mediaUrlRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecipeService(IRecipeRepository recipeRepository,
            IMediaUrlRepository mediaUrlRepository,
            IIngredientRepository ingredientRepository,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _recipeRepository = recipeRepository;
            _mediaUrlRepository = mediaUrlRepository;
            _ingredientRepository = ingredientRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<RecipeDTO>> CreateRecipeAsync(Guid userId, RecipeDTO recipeDTO, CancellationToken cancellationToken)
        {
            Result<MediaUrl?> result = await _mediaUrlRepository.GetMediaUrlById(recipeDTO.RecipeCoverImage.Id, cancellationToken);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<RecipeDTO>.Failure("Failed to create recipe because media url for cover image not found", ErrorCode.RecipeCreationFailed, result.Exception);
            }

            MediaUrl mediaUrl = result.Data;

            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Result<RecipeDTO>.Failure("User not found", ErrorCode.UserNotFound);
            }

            // Retrieve Ingredient entities by IDs from the database
            var ingredientIds = recipeDTO.IngredientDTOs.Select(i => i.Id).ToList();
            var ingredientsResult = await _ingredientRepository.GetIngredientsByIdsAsync(ingredientIds, cancellationToken);

            if (!ingredientsResult.IsSuccess)
            {
                return Result<RecipeDTO>.Failure("One or more ingredients not found", ErrorCode.RecipeCreationFailed);
            }

            // Ensure all requested ingredients were found
            if (ingredientsResult.Data!.Count != ingredientIds.Count)
            {
                return Result<RecipeDTO>.Failure("One or more ingredients not found", ErrorCode.RecipeCreationFailed);
            }


            Recipe recipe = new Recipe
            {
                Id = Guid.NewGuid(),
                Title = recipeDTO.Title,
                Description = recipeDTO.Description,
                Cuisine = recipeDTO.Cuisine,
                Difficulty = Enum.TryParse<DifficultyEnum>(recipeDTO.Difficulty.ToString(), out var difficulty)
                                ? difficulty
                                : DifficultyEnum.EASY,
                CookTimeInMinutes = recipeDTO.CookTimeInMinutes,
                Serving = recipeDTO.Serving,
                CreatedBy = user,
                RecipeCoverImage = mediaUrl,
                DateCreated = DateTime.Now,
                Ingredients = ingredientsResult.Data,
                RecipeSteps = recipeDTO.StepDTOs.Select(s => new RecipeStep
                {
                    Id = Guid.NewGuid(),
                    StepNumber = s.Order,
                    Instruction = s.Text,
                    MediaUrls = new List<MediaUrl>
                    {
                        !string.IsNullOrEmpty(s.ImageId) ? new MediaUrl
                        {
                            Id = Guid.Parse(s.ImageId),
                            Url = s.ImageUrl,
                            Alt = s.ImageUrl
                        } : new MediaUrl(),
                        !string.IsNullOrEmpty(s.VideoId) ? new MediaUrl
                        {
                            Id = Guid.Parse(s.VideoId),
                            Url = s.VideoUrl,
                            Alt = s.VideoUrl
                        } : new MediaUrl()
                    }.Where(media => media.Url != string.Empty).ToList() // Filter out default fallback
                }).ToList()
            };

            Result<Recipe> createResult = await _recipeRepository.CreateRecipeAsync(recipe, cancellationToken);

            if (!createResult.IsSuccess)
            {
                return Result<RecipeDTO>.Failure("Failed to create recipe.", ErrorCode.RecipeCreationFailed, createResult.Exception);
            }

            int saveChangesResult;
            try
            {
                saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken); // Save changes

                if (saveChangesResult <= 0)
                {
                    return Result<RecipeDTO>.Failure("No changes were made to the database.", ErrorCode.RecipeCreationFailed);
                }
            }
            catch (Exception saveEx)
            {
                return Result<RecipeDTO>.Failure("Failed to save recipe changes.", ErrorCode.RecipeCreationFailed, saveEx);
            }

            return Result<RecipeDTO>.Success(recipeDTO, "Create recipe successfully");

        }

        public async Task<Result<List<RecipeDTO>>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            Result<List<Recipe>> result = await _recipeRepository.GetAllRecipeOverviewByUserIdAsync(userId, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<List<RecipeDTO>>.Failure(
                    "Failed to retrieve recipes",
                    ErrorCode.RecipeRetrievalFailed,
                    result.Exception);
            }

            return Result<List<RecipeDTO>>.Success(
                result.Data!.Select(r => new RecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Cuisine = r.Cuisine,
                    Difficulty = r.Difficulty.ToString(),
                    CookTimeInMinutes = r.CookTimeInMinutes,
                    Serving = r.Serving,
                    CreatedBy = r.CreatedBy.Id,
                    RecipeCoverImage = new MediaUrlDTO
                    {
                        Id = r.RecipeCoverImage!.Id,
                        Url = r.RecipeCoverImage.Url,
                        Alt = r.RecipeCoverImage.Alt
                    },
                    DateCreated = r.DateCreated,
                }).ToList()
            );
        }
    }
}
