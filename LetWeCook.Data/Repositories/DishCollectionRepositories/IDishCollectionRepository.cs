using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.DishCollectionRepositories
{
    public interface IDishCollectionRepository
    {
        /// <summary>
        /// Asynchronously creates a new dish collection and adds it to the database.
        /// </summary>
        /// <param name="dishCollection">The dish collection object to be created and added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="DishCollection"/> object that was created and added to the database.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="dishCollection"/> is <c>null</c> or invalid.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<DishCollection> CreateDishCollectionAsync(DishCollection dishCollection, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves all dish collections associated with a particular user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose dish collections are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of <see cref="DishCollection"/> entities.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="userId"/> is <c>null</c> or invalid.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<DishCollection>> GetAllDishCollectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a single dish collection by its unique identifier.
        /// </summary>
        /// <param name="collectionId">The unique identifier of the dish collection to retrieve.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="DishCollection"/> entity if found, or <c>null</c> if no such collection exists.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<DishCollection?> GetDishCollectionByIdAsync(Guid collectionId, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously deletes a dish collection from the database by its unique identifier.
        /// </summary>
        /// <param name="collectionId">The unique identifier of the dish collection to delete.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the deletion was successful.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="collectionId"/> is <c>null</c> or invalid.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<bool> DeleteDishCollectionAsync(Guid collectionId, CancellationToken cancellationToken);

        Task<DishCollection?> GetDishCollectionDetailsByIdAsync(Guid collectionId, CancellationToken cancellationToken);
    }
}
