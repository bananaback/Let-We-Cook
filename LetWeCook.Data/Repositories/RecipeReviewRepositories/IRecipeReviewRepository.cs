using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.RecipeReviewRepositories
{
    public interface IRecipeReviewRepository
    {
        /// <summary>
        /// Get all reviews for a recipe.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of recipe reviews.</returns>
        Task<List<RecipeReview>> GetAllReviewsByRecipeIdAsync(Guid recipeId, CancellationToken cancellationToken);

        /// <summary>
        /// Get a review by its ID.
        /// </summary>
        /// <param name="reviewId">The ID of the review.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The review entity.</returns>
        Task<RecipeReview?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken);

        /// <summary>
        /// Get a user's review for a recipe.
        /// </summary>
        /// <param name="recipeId">The ID of the recipe.</param>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The user's review for the recipe.</returns>
        Task<RecipeReview?> GetUserReviewByRecipeIdAsync(Guid recipeId, string userId, CancellationToken cancellationToken);

        /// <summary>
        /// Add a new review for a recipe if the user hasn't reviewed it yet.
        /// </summary>
        /// <param name="review">The review entity to add.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The added review entity.</returns>
        Task<RecipeReview> AddReviewAsync(RecipeReview review, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing review.
        /// </summary>
        /// <param name="review">The review entity to update.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The updated review entity.</returns>
        Task<RecipeReview> UpdateReviewAsync(RecipeReview review, CancellationToken cancellationToken);

        /// <summary>
        /// Delete a review by its ID.
        /// </summary>
        /// <param name="reviewId">The ID of the review to delete.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A boolean indicating whether the review was deleted.</returns>
        Task<bool> DeleteReviewAsync(Guid reviewId, CancellationToken cancellationToken);
    }
}
