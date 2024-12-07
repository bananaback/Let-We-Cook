using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.RecipeReviewServices
{
    public interface IRecipeReviewService
    {
        Task<RecipeReviewDTO> CreateReviewForUser(string userId, Guid recipeId, string review, decimal rating, CancellationToken cancellationToken);
        Task<List<RecipeReviewDTO>> GetReviewsForRecipe(Guid recipeId, CancellationToken cancellationToken);
        Task<bool> DeleteReviewForUser(string userId, Guid reviewId, CancellationToken cancellationToken);

    }
}
