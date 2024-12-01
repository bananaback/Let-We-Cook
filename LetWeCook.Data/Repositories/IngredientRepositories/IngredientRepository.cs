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

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken = default)
        {
            await _context.Ingredients.AddAsync(ingredient, cancellationToken);
            return ingredient;
        }



        public async Task<List<Ingredient>> GetAllIngredientIdsAndNamesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Select(i => new Ingredient { Id = i.Id, Name = i.Name }) // Populate only ID and Name
                .ToListAsync(cancellationToken);
        }


        public async Task<List<Ingredient>> GetIngredientsWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Include(i => i.CoverImageUrl)                 // Eagerly load CoverImageUrl
                .Include(i => i.IngredientSections)            // Eagerly load IngredientSections
                .ToListAsync(cancellationToken);
        }



        public async Task<Ingredient?> GetIngredientWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Include(i => i.CoverImageUrl)                // Eagerly load CoverImageUrl
                .Include(i => i.IngredientSections)           // Eagerly load IngredientSections
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }



        public async Task<List<Ingredient>> GetIngredientsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids), "The list of ingredient IDs cannot be null.");

            return await _context.Ingredients
                .Where(i => ids.Contains(i.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
