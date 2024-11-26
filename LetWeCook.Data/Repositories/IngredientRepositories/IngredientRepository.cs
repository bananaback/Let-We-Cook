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

        /// <summary>
        /// Asynchronously creates a new ingredient and adds it to the database context.
        /// </summary>
        /// <param name="ingredient">The ingredient to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The created <see cref="Ingredient"/>.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken = default)
        {
            await _context.Ingredients.AddAsync(ingredient, cancellationToken);
            return ingredient;
        }


        /// <summary>
        /// Asynchronously retrieves all ingredients with only their IDs and names populated.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects with only the ID and Name fields populated.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<Ingredient>> GetAllIngredientIdsAndNamesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Select(i => new Ingredient { Id = i.Id, Name = i.Name }) // Populate only ID and Name
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves all ingredients, including their cover image URLs and sections.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects, including their cover image URLs and sections.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<Ingredient>> GetIngredientsWithDetailsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Include(i => i.CoverImageUrl)                 // Eagerly load CoverImageUrl
                .Include(i => i.IngredientSections)            // Eagerly load IngredientSections
                .ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Asynchronously retrieves an ingredient by its ID, including its cover image URL and sections.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="Ingredient"/> with the specified ID, including its cover image URL and sections, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<Ingredient?> GetIngredientWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Include(i => i.CoverImageUrl)                // Eagerly load CoverImageUrl
                .Include(i => i.IngredientSections)           // Eagerly load IngredientSections
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }


        /// <summary>
        /// Asynchronously retrieves a list of ingredients by their IDs.
        /// </summary>
        /// <param name="ids">A list of unique identifiers for the ingredients.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects with the specified IDs.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="ids"/> list is null or the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<Ingredient>> GetIngredientsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids), "The list of ingredient IDs cannot be null.");

            return await _context.Ingredients
                .Where(i => ids.Contains(i.Id))
                .ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Asynchronously retrieves a list of ingredients for data export, including their cover image URLs and sections.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects for data export, including cover image URLs and sections.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<Ingredient>> GetIngredientsForExportAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Include(i => i.CoverImageUrl)                // Eagerly load CoverImageUrl
                .Include(i => i.IngredientSections)           // Eagerly load IngredientSections
                .OrderBy(i => i.Name)                         // Order ingredients by Name
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously retrieves an ingredient by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="Ingredient"/> object if found, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<Ingredient?> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Ingredients
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

    }
}
