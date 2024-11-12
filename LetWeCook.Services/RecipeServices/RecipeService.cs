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

        public async Task<Result<RecipeDTO>> CreateRecipeAsync(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken)
        {
            Result<MediaUrl?> getCoverImageUrlResult = await _mediaUrlRepository.GetMediaUrlById(recipeDTO.RecipeCoverImage.Id, cancellationToken);

            if (!getCoverImageUrlResult.IsSuccess || getCoverImageUrlResult.Data == null)
            {
                return Result<RecipeDTO>.Failure(
                    "Failed to create recipe because media url for cover image not found",
                    ErrorCode.RecipeCreationFailed, getCoverImageUrlResult.Exception);
            }

            MediaUrl coverImageMediaUrl = getCoverImageUrlResult.Data;

            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<RecipeDTO>.Failure("User not found", ErrorCode.UserNotFound);
            }

            // ^ code above is fine now, fix under

            // Retrieve Ingredient entities by IDs from the database
            var ingredientIds = recipeDTO.RecipeIngredientDTOs.Select(ri => ri.IngredientId).ToList();
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

            var ingredientMap = ingredientsResult.Data.ToDictionary(i => i.Id, i => i);

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
                RecipeCoverImage = coverImageMediaUrl,
                DateCreated = DateTime.Now,
            };

            try
            {
                var recipeIngredientList = recipeDTO.RecipeIngredientDTOs.Select(
                    ri => new RecipeIngredient
                    {
                        Recipe = recipe,
                        Ingredient = ingredientMap[ri.IngredientId],
                        Quantity = ri.Quantity,
                        Unit = (UnitEnum)Enum.Parse(typeof(UnitEnum), ri.Unit)
                    }
                ).ToList();

                recipe.RecipeIngredients = recipeIngredientList;
            }
            catch (KeyNotFoundException ex)
            {
                return Result<RecipeDTO>.Failure("One or more ingredients not found", ErrorCode.RecipeCreationFailed, ex);
            }
            catch (FormatException ex)
            {
                return Result<RecipeDTO>.Failure("Invalid unit enum", ErrorCode.RecipeCreationFailed, ex);
            }
            catch (Exception ex)
            {
                return Result<RecipeDTO>.Failure("Failed to create recipe", ErrorCode.RecipeCreationFailed, ex);
            }

            List<Guid> stepMediaGuids;

            try
            {
                stepMediaGuids = recipeDTO.StepDTOs
                    .SelectMany(s => new[] { s.ImageId, s.VideoId })
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Distinct()
                    .Select(id => Guid.Parse(id)) // This will throw if id is invalid
                    .ToList();
            }
            catch (FormatException ex)
            {
                return Result<RecipeDTO>.Failure(
                    "Invalid media ID found.",
                    ErrorCode.RecipeCreationFailed,
                    ex); // Return failure if any ID is invalid
            }

            var stepMediaUrlRetrievalResult = await _mediaUrlRepository.GetMediaUrlByIdList(stepMediaGuids, cancellationToken);

            if (!stepMediaUrlRetrievalResult.IsSuccess || stepMediaUrlRetrievalResult.Data!.Count != stepMediaGuids.Count)
            {
                return Result<RecipeDTO>.Failure(
                    "Invalid media ID found.",
                    ErrorCode.RecipeCreationFailed,
                    stepMediaUrlRetrievalResult.Exception); // Return failure if any ID is invalid
            }


            // as this point, I think the id and corresponding media url is valid, and we dont need to try get value?
            var mediaUrlMap = stepMediaUrlRetrievalResult.Data!.ToDictionary(m => m.Id, m => m);

            var stepDTOMap = recipeDTO.StepDTOs.ToDictionary(s => s.Order);

            var recipeSteps = recipeDTO.StepDTOs.Select(s => new RecipeStep
            {
                Id = Guid.NewGuid(),
                Recipe = recipe,
                StepNumber = s.Order,
                Instruction = s.Text
            }).ToList();

            foreach (var recipeStep in recipeSteps)
            {
                if (!stepDTOMap.TryGetValue(recipeStep.StepNumber, out var stepDTO))
                {
                    return Result<RecipeDTO>.Failure(
                        "StepDTO not found for step number " + recipeStep.StepNumber,
                        ErrorCode.RecipeCreationFailed);
                }

                // Create RecipeStepMedias, only adding non-null items
                var recipeStepMedias = new List<RecipeStepMedia>();

                if (!string.IsNullOrEmpty(stepDTO.ImageId) && mediaUrlMap.TryGetValue(Guid.Parse(stepDTO.ImageId), out var imageMedia))
                {
                    recipeStepMedias.Add(new RecipeStepMedia { RecipeStep = recipeStep, MediaUrl = imageMedia });
                }

                if (!string.IsNullOrEmpty(stepDTO.VideoId) && mediaUrlMap.TryGetValue(Guid.Parse(stepDTO.VideoId), out var videoMedia))
                {
                    recipeStepMedias.Add(new RecipeStepMedia { RecipeStep = recipeStep, MediaUrl = videoMedia });
                }

                recipeStep.RecipeStepMedias = recipeStepMedias;
            }

            recipe.RecipeSteps = recipeSteps;

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
