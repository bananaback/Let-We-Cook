using LetWeCook.Common;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.RecipeReviewServices;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private const int MaxLevenshteinDistance = 10; // Adjust as needed for fuzzy tolerance

        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMediaUrlRepository _mediaUrlRepository;
        private readonly IRecipeReviewService _recipeReviewService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public RecipeService(IRecipeRepository recipeRepository,
            IMediaUrlRepository mediaUrlRepository,
            IIngredientRepository ingredientRepository,
            IRecipeReviewService recipeReviewService,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _recipeRepository = recipeRepository;
            _mediaUrlRepository = mediaUrlRepository;
            _ingredientRepository = ingredientRepository;
            _recipeReviewService = recipeReviewService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<RecipeDTO> CreateRecipeAsync(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken)
        {
            MediaUrl? mediaUrl = await _mediaUrlRepository.GetMediaUrlByIdAsync(recipeDTO.RecipeCoverImage.Id, cancellationToken);

            if (mediaUrl == null)
            {
                throw new RecipeCreationException("Failed to create recipe because media url for cover image not found");
            }


            ApplicationUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with id {userId} not found.");
            }

            // ^ code above is fine now, fix under

            // Retrieve Ingredient entities by IDs from the database
            var ingredientIds = recipeDTO.RecipeIngredientDTOs.Select(ri => ri.IngredientId).ToList();
            var ingredients = await _ingredientRepository.GetIngredientsByIdsAsync(ingredientIds, cancellationToken);

            // Ensure all requested ingredients were found
            if (ingredients.Count != ingredientIds.Count)
            {
                throw new RecipeCreationException("One or more ingredients not found");
            }

            var ingredientMap = ingredients.ToDictionary(i => i.Id, i => i);

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
                throw new RecipeCreationException("One or more ingredients not found", ex);
            }
            catch (FormatException ex)
            {
                throw new RecipeCreationException("Invalid unit enum", ex);
            }
            catch (Exception ex)
            {
                throw new RecipeCreationException("Failed to create recipe", ex);
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
                throw new RecipeCreationException
                ("Invalid media ID found.", ex); // Return failure if any ID is invalid
            }

            var stepMediaUrlRetrievalResult = await _mediaUrlRepository.GetMediaUrlsByIdsAsync(stepMediaGuids, cancellationToken);

            if (stepMediaUrlRetrievalResult.Count != stepMediaGuids.Count)
            {
                throw new RecipeCreationException(
                    "Invalid media ID found."); // Return failure if any ID is invalid
            }


            // as this point, I think the id and corresponding media url is valid, and we dont need to try get value?
            var mediaUrlMap = stepMediaUrlRetrievalResult.ToDictionary(m => m.Id, m => m);

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
                    throw new RecipeCreationException(
                        "StepDTO not found for step number " + recipeStep.StepNumber
                    );
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

            var createdRecipe = await _recipeRepository.CreateRecipeAsync(recipe, cancellationToken);

            int saveChangesResult;
            try
            {
                saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken); // Save changes

                if (saveChangesResult <= 0)
                {
                    throw new RecipeCreationException("No changes were made to the database.");
                }
            }
            catch (Exception saveEx)
            {
                throw new RecipeCreationException("Failed to save recipe changes.", saveEx);
            }

            return recipeDTO;
        }

        public async Task<List<RecipeDTO>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Recipe> recipes = await _recipeRepository.GetRecipeOverviewByUserIdAsync(userId, cancellationToken);

                return recipes.Select(r => new RecipeDTO
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
                }).ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw new RecipeRetrievalException($"Failed to retrieve recipe overview of user with id {userId}", ex);
            }
        }

        public async Task<List<RecipeDTO>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Recipe> recipes = await _recipeRepository.GetNewestRecipesAsync(count, cancellationToken);
                return recipes.Select(r => new RecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Cuisine = r.Cuisine,
                    Difficulty = r.Difficulty.ToString(),
                    CookTimeInMinutes = r.CookTimeInMinutes,
                    Serving = r.Serving,
                    RecipeCoverImage = new MediaUrlDTO
                    {
                        Id = r.RecipeCoverImage!.Id,
                        Url = r.RecipeCoverImage.Url,
                        Alt = r.RecipeCoverImage.Alt
                    },
                    DateCreated = r.DateCreated,
                }).ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw new RecipeRetrievalException("Failed to retrieve newest recipes", ex);
            }
        }


        public async Task<List<RecipeDTO>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Recipe> recipes = await _recipeRepository.GetRandomRecipesAsync(count, cancellationToken);
                return recipes.Select(r => new RecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Cuisine = r.Cuisine,
                    Difficulty = r.Difficulty.ToString(),
                    CookTimeInMinutes = r.CookTimeInMinutes,
                    Serving = r.Serving,
                    RecipeCoverImage = new MediaUrlDTO
                    {
                        Id = r.RecipeCoverImage!.Id,
                        Url = r.RecipeCoverImage.Url,
                        Alt = r.RecipeCoverImage.Alt
                    },
                    DateCreated = r.DateCreated,
                }).ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw new RecipeRetrievalException("Failed to retrieve random recipes", ex);

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new RecipeRetrievalException("Failed to retrieve random recipes", ex);

            }

        }


        public async Task<RecipeDTO> GetRecipeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Recipe? recipe = await _recipeRepository.GetRecipeDetailsByIdAsync(id, cancellationToken);

            if (recipe == null)
            {
                throw new RecipeRetrievalException($"Recipe with id {id} not found.");
            }

            List<RecipeReviewDTO> reviews = await _recipeReviewService.GetReviewsForRecipe(id, cancellationToken);

            return new RecipeDTO
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Cuisine = recipe.Cuisine,
                Difficulty = recipe.Difficulty.ToString(),
                CookTimeInMinutes = recipe.CookTimeInMinutes,
                Serving = recipe.Serving,
                CreatedBy = recipe.CreatedBy.Id,
                RecipeCoverImage = new MediaUrlDTO
                {
                    Id = recipe.RecipeCoverImage!.Id,
                    Url = recipe.RecipeCoverImage.Url,
                    Alt = recipe.RecipeCoverImage.Alt
                },
                RecipeIngredientDTOs = recipe.RecipeIngredients.Select(ri => new RecipeIngredientDTO
                {
                    IngredientId = ri.Ingredient.Id,
                    IngredientName = ri.Ingredient.Name,
                    Quantity = ri.Quantity,
                    Unit = ri.Unit.ToString()
                }).ToList(),
                StepDTOs = recipe.RecipeSteps.Select(rs =>
                {
                    StepDTO stepDTO = new StepDTO
                    {
                        Id = rs.Id,
                        Text = rs.Instruction,
                        Order = rs.StepNumber,

                    };

                    if (rs.RecipeStepMedias.Count != 0)
                    {
                        MediaUrl mediaUrl = rs.RecipeStepMedias[0].MediaUrl;
                        if (mediaUrl.Url.EndsWith(".mp4"))
                        {
                            stepDTO.VideoUrl = mediaUrl.Url;
                            stepDTO.VideoId = mediaUrl.Id.ToString();
                        }
                        else
                        {
                            stepDTO.ImageUrl = mediaUrl.Url;
                            stepDTO.ImageId = mediaUrl.Id.ToString();
                        }
                    }

                    return stepDTO;
                }).ToList(),
                DateCreated = recipe.DateCreated,
                RecipeReviews = reviews
            };
        }

        public async Task<List<RecipeDTO>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Recipe> recipes = await _recipeRepository.GetRecipesByCuisineAsync(cuisine, count, cancellationToken);
                return recipes.Select(r => new RecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Cuisine = r.Cuisine,
                    Difficulty = r.Difficulty.ToString(),
                    CookTimeInMinutes = r.CookTimeInMinutes,
                    Serving = r.Serving,
                    RecipeCoverImage = new MediaUrlDTO
                    {
                        Id = r.RecipeCoverImage!.Id,
                        Url = r.RecipeCoverImage.Url,
                        Alt = r.RecipeCoverImage.Alt
                    },
                    DateCreated = r.DateCreated,
                }).ToList();
            }
            catch (ArgumentNullException ex)
            {
                throw new RecipeRetrievalException("Failed to retrieve recipes by cuisine", ex);

            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new RecipeRetrievalException("Failed to retrieve recipes by cuisine", ex);

            }
        }


        public async Task<List<RecipeDTO>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<List<RecipeDTO>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<PaginatedResult<RecipeDTO>> SearchRecipesAsync(
            string searchTerm = "",
            string cuisine = "",
            string difficulty = "",
            int cookTime = 0,
            int servings = 0,
            string sortBy = "",
            int itemsPerPage = 10,
            int currentPage = 1,
            CancellationToken cancellationToken = default)
        {
            if (itemsPerPage <= 0)
            {
                throw new RecipeRetrievalException($"Invalid page size: {itemsPerPage}");
            }


            try
            {
                var recipes = await _recipeRepository.GetRecipeOverviewsAsync(cancellationToken);

                // Apply fuzzy search using Levenshtein distance
                var filteredRecipes = string.IsNullOrEmpty(searchTerm)
                    ? recipes
                    : recipes!
                        .Where(i => i.Title.LevenshteinDistance(searchTerm) <= MaxLevenshteinDistance)
                        .ToList();

                // Apply additional filters
                if (!string.IsNullOrEmpty(cuisine))
                {
                    filteredRecipes = filteredRecipes
                        .Where(r => r.Cuisine.Equals(cuisine, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (!string.IsNullOrEmpty(difficulty))
                {
                    filteredRecipes = filteredRecipes
                        .Where(r => r.Difficulty.ToString().Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                if (cookTime > 0)
                {
                    filteredRecipes = filteredRecipes
                        .Where(r => r.CookTimeInMinutes <= cookTime)
                        .ToList();
                }

                if (servings > 0)
                {
                    filteredRecipes = filteredRecipes
                        .Where(r => Math.Abs(r.Serving - servings) <= 3)
                        .ToList();
                }

                int totalItems = filteredRecipes.Count;

                var pagedRecipes = filteredRecipes
                    .Skip((currentPage - 1) * itemsPerPage)
                    .Take(itemsPerPage)
                    .Select(r => new RecipeDTO
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
                        RecipeIngredientDTOs = r.RecipeIngredients.Select(ri => new RecipeIngredientDTO
                        {
                            IngredientId = ri.Ingredient.Id,
                            IngredientName = ri.Ingredient.Name,
                            Quantity = ri.Quantity,
                            Unit = ri.Unit.ToString()
                        }).ToList(),
                        StepDTOs = r.RecipeSteps.Select(rs =>
                        {
                            StepDTO stepDTO = new StepDTO
                            {
                                Id = rs.Id,
                                Text = rs.Instruction,
                                Order = rs.StepNumber,
                            };

                            if (rs.RecipeStepMedias.Count != 0)
                            {
                                MediaUrl mediaUrl = rs.RecipeStepMedias[0].MediaUrl;
                                if (mediaUrl.Url.EndsWith(".mp4"))
                                {
                                    stepDTO.VideoUrl = mediaUrl.Url;
                                    stepDTO.VideoId = mediaUrl.Id.ToString();
                                }
                                else
                                {
                                    stepDTO.ImageUrl = mediaUrl.Url;
                                    stepDTO.ImageId = mediaUrl.Id.ToString();
                                }
                            }

                            return stepDTO;
                        }).ToList(),
                        DateCreated = r.DateCreated,
                    })
                    .ToList();

                var paginatedResult = new PaginatedResult<RecipeDTO>(pagedRecipes, currentPage, itemsPerPage, totalItems);
                return paginatedResult;
            }
            catch (ArgumentNullException ex)
            {
                throw new RecipeRetrievalException("Failed to search recipe overviews", ex);
            }



        }

    }
}
