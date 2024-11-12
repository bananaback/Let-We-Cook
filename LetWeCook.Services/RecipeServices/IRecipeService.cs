using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<Result<List<RecipeDTO>>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<RecipeDTO>> CreateRecipeAsync(string userId, RecipeDTO recipeDTO, CancellationToken cancellationToken);
    }
}
