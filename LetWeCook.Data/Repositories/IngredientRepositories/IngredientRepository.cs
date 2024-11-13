using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.IngredientRepositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly LetWeCookDbContext _context;
        public IngredientRepository(LetWeCookDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Ingredient>> CreateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken)
        {
            try
            {
                await _context.Ingredients.AddAsync(ingredient);

                return Result<Ingredient>.Success(ingredient, "Media URL created successfully.");
            }
            catch (Exception ex)
            {
                return Result<Ingredient>.Failure("An error occurred while creating the media URL.", ErrorCode.IngredientCreationFailed, ex);
            }
        }

        public async Task<Result<List<Ingredient>>> GetAllIngredientsNameAndIdAsync(CancellationToken cancellationToken)
        {
            List<Ingredient> ingredients = await _context.Ingredients.ToListAsync(cancellationToken);

            return Result<List<Ingredient>>.Success(ingredients, "Retrie all ingredients name and id successfully");
        }

        public async Task<Result<List<Ingredient>>> GetAllIngredientsWithCoverImageAndSectionsAsync(CancellationToken cancellationToken)
        {
            List<Ingredient> ingredients = await _context.Ingredients
                .Include(i => i.CoverImageUrl)                 // Eagerly load CoverImageUrl
                .Include(i => i.IngredientSections)            // Eagerly load IngredientSections
                .ToListAsync(cancellationToken);

            return Result<List<Ingredient>>.Success(ingredients, "Retrieved all ingredients successfully.");
        }

        public async Task<Result<Ingredient>> GetInredientWithCoverImageAndSectionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                Ingredient? ingredient = await _context.Ingredients
               .Include(i => i.CoverImageUrl)
               .Include(i => i.IngredientSections)
               .Where(i => i.Id == id)
               .FirstOrDefaultAsync(cancellationToken);

                if (ingredient == null)
                {
                    return Result<Ingredient>.Failure(
                        $"Failed to retrieve ingredient with id {id}",
                        ErrorCode.IngredientNotFound);

                }

                return Result<Ingredient>.Success(ingredient, "Retrieve ingredient success.");
            }
            catch (Exception ex)
            {
                return Result<Ingredient>.Failure("Failed to retrieve ingredient", ErrorCode.IngredientRetrievalFailed, ex);
            }
        }

        public async Task<Result<List<Ingredient>>> GetIngredientsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
        {
            try
            {
                List<Ingredient> ingredients = await _context.Ingredients
                    .Where(i => ids.Contains(i.Id))
                    .ToListAsync(cancellationToken);

                return Result<List<Ingredient>>.Success(ingredients, "Ingredients retrieved successfully by IDs.");
            }
            catch (Exception ex)
            {
                return Result<List<Ingredient>>.Failure("Failed to retrieve ingredients by IDs", ErrorCode.IngredientRetrievalFailed, ex);
            }
        }
    }
}
