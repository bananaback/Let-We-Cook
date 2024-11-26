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

        /// <summary>
        /// Asynchronously retrieves an ingredient section by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient section.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IngredientSection"/> object if found, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<IngredientSection?> GetIngredientSectionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.IngredientSections
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
