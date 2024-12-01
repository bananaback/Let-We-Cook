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

        public async Task<IngredientSection> AddIngredientSectionAsync(IngredientSection ingredientSection, CancellationToken cancellationToken)
        {
            await _context.IngredientSections.AddAsync(ingredientSection, cancellationToken);
            return ingredientSection;
        }

        public async Task<IngredientSection?> GetIngredientSectionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.IngredientSections
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
