using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.CollectionRecipeRepositories
{
    public interface ICollectionRecipeRepository
    {
        /// <summary>
        /// Asynchronously creates a new collection recipe and adds it to the database.
        /// </summary>
        /// <param name="collectionRecipe">The collection recipe object to be created and added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="CollectionRecipe"/> object that was created and added to the database.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="collectionRecipe"/> is <c>null</c> or invalid.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<CollectionRecipe> CreateCollectionRecipeAsync(CollectionRecipe collectionRecipe, CancellationToken cancellationToken);

        Task DeleteCollectionRecipeAsync(Guid collectionId, Guid recipeId, CancellationToken cancellationToken);
    }
}
