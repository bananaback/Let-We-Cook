using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.IngredientSectionRepositories
{
    public class IngredientSectionRepository : IIngredientSectionRepository
    {
        private readonly LetWeCookDbContext _context;

        public IngredientSectionRepository(LetWeCookDbContext context)
        {
            _context = context;
        }
        public async Task<Result<IngredientSection>> GetIngredientSectionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                IngredientSection? ingredientSection = await _context.IngredientSections
               .Where(i => i.Id == id)
               .FirstOrDefaultAsync(cancellationToken);

                if (ingredientSection == null)
                {
                    return Result<IngredientSection>.Failure(
                        $"Failed to retrieve ingredient section with id {id}",
                        ErrorCode.IngredientNotFound);

                }

                return Result<IngredientSection>.Success(ingredientSection, "Retrieve ingredient section success.");
            }
            catch (Exception ex)
            {
                return Result<IngredientSection>.Failure("Failed to retrieve ingredient section", ErrorCode.IngredientSectionRetrievalFailed, ex);
            }
        }
    }
}
