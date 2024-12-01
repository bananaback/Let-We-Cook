using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.IngredientRepositories
{
    public interface IIngredientRepository
    {
        /// <summary>
        /// Asynchronously creates a new ingredient and adds it to the database context.
        /// </summary>
        /// <param name="ingredient">The ingredient to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The created <see cref="Ingredient"/>.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves all ingredients, including their cover image URLs and sections.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects, including their cover image URLs and sections.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Ingredient>> GetIngredientsWithDetailsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves an ingredient by its ID, including its cover image URL and sections.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="Ingredient"/> with the specified ID, including its cover image URL and sections, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<Ingredient?> GetIngredientWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves all ingredients with only their IDs and names populated.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects with only the ID and Name fields populated.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Ingredient>> GetAllIngredientIdsAndNamesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a list of ingredients by their IDs.
        /// </summary>
        /// <param name="ids">A list of unique identifiers for the ingredients.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of <see cref="Ingredient"/> objects with the specified IDs.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="ids"/> list is null or the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Ingredient>> GetIngredientsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);


        /// <summary>
        /// Asynchronously retrieves an ingredient by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ingredient.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="Ingredient"/> object if found, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<Ingredient?> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken);

    }
}
