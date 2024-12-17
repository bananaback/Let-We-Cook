using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<List<RecipeDTO>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<RecipeDTO> CreateRecipeAsync(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken);
        Task<RecipeDTO> GetRecipeByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<PaginatedResult<RecipeDTO>> SearchRecipesAsync(string searchTerm, string cuisine, string difficulty, int cookTime, int servings, string sortBy, int itemsPerPage, int currentPage, CancellationToken cancellationToken);

        Task<List<RecipeDTO>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken);

        Task<List<RecipeDTO>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken);

        Task<List<RecipeDTO>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken);

        Task<List<RecipeDTO>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken);

        Task<List<RecipeDTO>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken);
        Task DeleteRecipeByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<RecipeDTO> UpdateRecipe(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken);
    }
}
