using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.RecipeReviewRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.RecipeReviewServices
{
    public class RecipeReviewService : IRecipeReviewService
    {
        private readonly IRecipeReviewRepository _recipeReviewRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeReviewService(IRecipeReviewRepository recipeReviewRepository,
            IRecipeRepository recipeRepository,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _recipeReviewRepository = recipeReviewRepository;
            _recipeRepository = recipeRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<RecipeReviewDTO> CreateReviewForUser(string userId, Guid recipeId, string review, decimal rating, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"User with id {userId} not found.");
            }

            var recipe = await _recipeRepository.GetRecipeDetailsByIdAsync(recipeId, cancellationToken);

            if (recipe == null)
            {
                throw new RecipeReviewCreationException($"Recipe with id {recipeId} not found.");
            }

            var existingReview = await _recipeReviewRepository.GetUserReviewByRecipeIdAsync(recipeId, userId, cancellationToken);

            if (existingReview != null)
            {
                await _recipeReviewRepository.DeleteReviewAsync(existingReview.Id, cancellationToken);
            }

            RecipeReview recipeReview = new RecipeReview
            {
                Id = Guid.NewGuid(),
                User = user,
                Recipe = recipe!,
                Review = review,
                Rating = rating,
                CreatedDate = DateTime.Now
            };

            var savedReview = await _recipeReviewRepository.AddReviewAsync(recipeReview, cancellationToken);

            // Save changes to ensure the review is persisted
            int res = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (res <= 0)
            {
                throw new RecipeReviewCreationException("Cannot save changes after create recipe review");
            }

            // Recalculate the average rating
            var reviews = await _recipeReviewRepository.GetAllReviewsByRecipeIdAsync(recipeId, cancellationToken);

            decimal averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : rating;

            recipe.AverageRating = averageRating;

            // Save updated recipe with the new average rating
            await _recipeRepository.UpdateRecipe(recipe);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RecipeReviewDTO
            {
                Id = savedReview.Id,
                Username = user.UserName ?? "Anonymous user",
                UserAvatarUrl = string.Empty,
                Review = review,
                Rating = rating,
                CreatedDate = savedReview.CreatedDate,
            };
        }


        public async Task<bool> DeleteReviewForUser(string userId, Guid reviewId, CancellationToken cancellationToken)
        {
            var existingReview = await _recipeReviewRepository.GetReviewByIdAsync(reviewId, cancellationToken);
            if (existingReview != null)
            {
                if (existingReview.User.Id.ToString() == userId)
                {
                    await _recipeReviewRepository.DeleteReviewAsync(reviewId, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    return true;
                }
            }
            return false;
        }

        public async Task<List<RecipeReviewDTO>> GetReviewsForRecipe(Guid recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipeDetailsByIdAsync(recipeId, cancellationToken);

            if (recipe == null)
            {
                throw new RecipeReviewCreationException($"Recipe with id {recipeId} not found.");
            }

            var reviews = await _recipeReviewRepository.GetAllReviewsByRecipeIdAsync(recipeId, cancellationToken);

            return reviews.Select(r => new RecipeReviewDTO
            {
                Id = r.Id,
                Username = r.User.UserName ?? "Anonymous user",
                UserAvatarUrl = string.Empty,
                Review = r.Review,
                Rating = r.Rating,
                CreatedDate = r.CreatedDate,
            }).ToList();
        }
    }
}
