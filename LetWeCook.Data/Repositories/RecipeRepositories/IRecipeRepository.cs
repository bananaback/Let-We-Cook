using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.RecipeRepositories
{
    public interface IRecipeRepository
    {
        Task<Result<List<Recipe>>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<Recipe>> CreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);
        Task<Result<Recipe>> GetRecipeWithCoverImageAndIngredientsAndStepsByRecipeIdAsync(Guid recipeId, CancellationToken cancellationToken);
        Task<Result<List<Recipe>>> GetAllRecipes(CancellationToken cancellationToken);

        Task<Result<List<Recipe>>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken); // For "Newest Recipes" section
        Task<Result<List<Recipe>>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken); // For "Random Recipes" section
        Task<Result<List<Recipe>>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken); // For "Featured Vietnamese Recipes" or similar sections
        Task<Result<List<Recipe>>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken); // For "Recipes by Difficulty" sections
        Task<Result<List<Recipe>>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken); // For "Trending Recipes" section (based on views/likes)
    }
}
