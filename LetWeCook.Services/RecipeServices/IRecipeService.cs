using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<Result<List<RecipeDTO>>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<RecipeDTO>> CreateRecipeAsync(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken);
        Task<Result<RecipeDTO>> GetRecipeByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<PaginatedResult<RecipeDTO>>> SearchRecipesAsync(string searchTerm, string cuisine, string difficulty, int cookTime, int servings, string sortBy, int itemsPerPage, int currentPage, CancellationToken cancellationToken);

        Task<Result<List<RecipeDTO>>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken);

        Task<Result<List<RecipeDTO>>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken);

        Task<Result<List<RecipeDTO>>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken);

        Task<Result<List<RecipeDTO>>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken);

        Task<Result<List<RecipeDTO>>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken);
    }
}
