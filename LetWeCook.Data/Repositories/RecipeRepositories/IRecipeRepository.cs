using LetWeCook.Data.Entities;


namespace LetWeCook.Data.Repositories.RecipeRepositories
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Recipe> CreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);
        Task<Recipe?> GetRecipeDetailsByIdAsync(Guid recipeId, CancellationToken cancellationToken);
        Task<List<Recipe>> GetAllRecipesAsync(CancellationToken cancellationToken);
        Task<List<Recipe>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken);
        Task<List<Recipe>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken);
        Task<List<Recipe>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken);
        Task<List<Recipe>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken);
        Task<List<Recipe>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken);
    }
}
