using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.IngredientSectionRepositories
{
    public interface IIngredientSectionRepository
    {
        /// <summary>
        /// Asynchronously retrieves an ingredient section by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient section.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IngredientSection"/> object if found, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<IngredientSection?> GetIngredientSectionByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously adds a new ingredient section to the database.
        /// </summary>
        /// <param name="ingredientSection">The ingredient section to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="IngredientSection"/> object that was added to the database.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<IngredientSection> AddIngredientSectionAsync(IngredientSection ingredientSection, CancellationToken cancellationToken);
    }
}
