using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.RecipeRepositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly LetWeCookDbContext _context;
        public RecipeRepository(LetWeCookDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Recipe>> CreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Recipes.AddAsync(recipe, cancellationToken);

                return Result<Recipe>.Success(recipe, "Recipe created successfully.");
            }
            catch (Exception ex)
            {
                return Result<Recipe>.Failure("An error occurred while creating the recipe.", ErrorCode.RecipeCreationFailed, ex);
            }
        }

        public async Task<Result<List<Recipe>>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                List<Recipe> recipes = await _context.Recipes
                    .Include(r => r.RecipeCoverImage)
                    .Include(r => r.CreatedBy)
                    .Where(r => r.CreatedBy.Id == userId)
                    .ToListAsync();
                return Result<List<Recipe>>.Success(recipes, "Retrieve recipes successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure("Failed to retrieve recipes", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }

        public async Task<Result<List<Recipe>>> GetAllRecipes(CancellationToken cancellationToken)
        {
            try
            {
                List<Recipe> recipes = await _context.Recipes
                    .Include(r => r.RecipeCoverImage)
                    .Include(r => r.CreatedBy)
                    .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .Include(r => r.RecipeSteps)
                    .ThenInclude(rs => rs.RecipeStepMedias)
                    .ToListAsync();
                return Result<List<Recipe>>.Success(recipes, "Retrieve recipes successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure("Failed to retrieve recipes", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }

        public async Task<Result<List<Recipe>>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken)
        {
            try
            {
                // Query the recipes sorted by the most recent creation date
                List<Recipe> newestRecipes = await _context.Recipes
                    .Include(r => r.RecipeCoverImage)
                    .OrderByDescending(r => r.DateCreated) // Sort by creation date in descending order
                    .Take(count) // Limit the number of results to `count`
                    .ToListAsync(cancellationToken);

                return Result<List<Recipe>>.Success(newestRecipes, "Retrieve newest recipes successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure("Failed to retrieve newest recipes", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }


        public async Task<Result<List<Recipe>>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the total number of recipes
                int totalRecipes = await _context.Recipes.CountAsync(cancellationToken);

                if (totalRecipes == 0)
                {
                    return Result<List<Recipe>>.Success(new List<Recipe>(), "No recipes available.");
                }

                // Generate random indices for recipes
                var randomIndices = Enumerable.Range(0, totalRecipes)
                    .OrderBy(_ => Guid.NewGuid()) // Shuffle using GUID for randomness
                    .Take(count)
                    .ToList();

                // Fetch recipes based on random indices
                List<Recipe> randomRecipes = await _context.Recipes
                    .Include(r => r.RecipeCoverImage)
                    .OrderBy(r => r.Id) // Ensure deterministic order
                    .Skip(randomIndices.Min()) // Efficiently skip to the lowest index
                    .Take(randomIndices.Max() - randomIndices.Min() + 1) // Fetch range
                    .ToListAsync(cancellationToken);

                // Finalize the random selection
                randomRecipes = randomRecipes
                    .OrderBy(_ => Guid.NewGuid()) // Shuffle again to ensure randomness
                    .Take(count)
                    .ToList();

                return Result<List<Recipe>>.Success(randomRecipes, "Retrieve random recipes successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure("Failed to retrieve random recipes", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }


        public async Task<Result<List<Recipe>>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch recipes filtered by cuisine
                List<Recipe> recipes = await _context.Recipes
                    .Where(r => r.Cuisine == cuisine) // Filter by cuisine
                    .Include(r => r.RecipeCoverImage)
                    .Take(count) // Limit to the specified count
                    .ToListAsync(cancellationToken);

                // Handle case when no recipes are found
                if (!recipes.Any())
                {
                    return Result<List<Recipe>>.Success(new List<Recipe>(), $"No recipes found for cuisine '{cuisine}'.");
                }

                return Result<List<Recipe>>.Success(recipes, $"Retrieve {recipes.Count} recipes for cuisine '{cuisine}' successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure($"Failed to retrieve recipes for cuisine '{cuisine}'", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }


        public async Task<Result<List<Recipe>>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken)
        {
            try
            {
                // Attempt to parse the difficulty into an enum if applicable
                if (!Enum.TryParse<DifficultyEnum>(difficulty, true, out var parsedDifficulty))
                {
                    return Result<List<Recipe>>.Failure($"Invalid difficulty level '{difficulty}'.", ErrorCode.RecipeRetrievalFailed);
                }

                // Fetch recipes filtered by difficulty
                List<Recipe> recipes = await _context.Recipes
                    .Where(r => r.Difficulty == parsedDifficulty) // Filter by difficulty
                    .Include(r => r.RecipeCoverImage)
                    .Include(r => r.CreatedBy)
                    .Include(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                    .Include(r => r.RecipeSteps)
                        .ThenInclude(rs => rs.RecipeStepMedias)
                    .OrderByDescending(r => r.DateCreated) // Sort by newest
                    .Take(count) // Limit to the specified count
                    .ToListAsync(cancellationToken);

                // Handle case when no recipes are found
                if (!recipes.Any())
                {
                    return Result<List<Recipe>>.Success(new List<Recipe>(), $"No recipes found for difficulty '{difficulty}'.");
                }

                return Result<List<Recipe>>.Success(recipes, $"Retrieve {recipes.Count} recipes for difficulty '{difficulty}' successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure($"Failed to retrieve recipes for difficulty '{difficulty}'", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }


        public async Task<Result<Recipe>> GetRecipeWithCoverImageAndIngredientsAndStepsByRecipeIdAsync(Guid recipeId, CancellationToken cancellationToken)
        {

            try
            {
                Recipe? recipe = await _context.Recipes
                    .Include(r => r.RecipeCoverImage)
                    .Include(r => r.CreatedBy)
                    .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                    .Include(r => r.RecipeSteps)
                    .ThenInclude(rs => rs.RecipeStepMedias)
                    .Where(r => r.Id == recipeId)
                    .FirstOrDefaultAsync();

                if (recipe == null)
                {
                    return Result<Recipe>.Failure(
                        $"Failed to retrieve recipe with id {recipeId}",
                        ErrorCode.RecipeNotFound);

                }

                return Result<Recipe>.Success(recipe, "Retrieve recipe success.");
            }
            catch (Exception ex)
            {
                return Result<Recipe>.Failure($"Failed to retrieve recipe with id {recipeId}", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }

        public Task<Result<List<Recipe>>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
