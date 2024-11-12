using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.RecipeRepositories
{
    public interface IRecipeRepository
    {
        Task<Result<List<Recipe>>> GetAllRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<Recipe>> CreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);
    }
}
