using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.RecipeReviewRepositories
{
    public class RecipeReviewRepository : IRecipeReviewRepository
    {
        private readonly LetWeCookDbContext _context;
        public RecipeReviewRepository(LetWeCookDbContext context)
        {
            _context = context;
        }

        public async Task<RecipeReview> AddReviewAsync(RecipeReview review, CancellationToken cancellationToken)
        {
            await _context.AddAsync(review, cancellationToken);
            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid reviewId, CancellationToken cancellationToken)
        {
            var review = await _context.RecipeReviews.Where(rr => rr.Id == reviewId).FirstOrDefaultAsync();
            if (review != null)
            {
                _context.RecipeReviews.Remove(review);
                return true;
            }
            return false;
        }

        public async Task<List<RecipeReview>> GetAllReviewsByRecipeIdAsync(Guid recipeId, CancellationToken cancellationToken)
        {
            return await _context.RecipeReviews
                .Include(rr => rr.Recipe)
                .Include(rr => rr.User)
                .Where(rr => rr.Recipe.Id == recipeId).ToListAsync(cancellationToken);
        }

        public async Task<RecipeReview?> GetReviewByIdAsync(Guid reviewId, CancellationToken cancellationToken)
        {
            return await _context.RecipeReviews
                .Include(rr => rr.User)
                .Where(rr => rr.Id == reviewId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<RecipeReview?> GetUserReviewByRecipeIdAsync(Guid recipeId, string userId, CancellationToken cancellationToken)
        {
            return await _context.RecipeReviews
                .Where(rr => rr.Recipe.Id == recipeId && rr.User.Id.ToString() == userId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<RecipeReview> UpdateReviewAsync(RecipeReview review, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
