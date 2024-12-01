using LetWeCook.Common;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private const int MaxLevenshteinDistance = 10; // Adjust as needed for fuzzy tolerance

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

        public async Task<RecipeDTO> CreateRecipeAsync(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
