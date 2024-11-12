using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
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
                    .Where(r => r.CreatedBy.Id == userId)
                    .ToListAsync();
                return Result<List<Recipe>>.Success(recipes, "Retrieve recipes successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<Recipe>>.Failure("Failed to retrieve recipes", ErrorCode.RecipeRetrievalFailed, ex);
            }
        }
    }
}
