using LetWeCook.Data.Entities;


namespace LetWeCook.Data.Repositories.RecipeRepositories
{
    public interface IRecipeRepository
    {
        /// <summary>
        /// Asynchronously retrieves a list of recipe overviews for a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The user ID whose recipes are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of recipes associated with the specified user.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="userId"/> is null or queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        Task<List<Recipe>> GetRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a list of recipe overviews.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of recipes overview.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        Task<List<Recipe>> GetRecipeOverviewsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously creates a new recipe in the database.
        /// </summary>
        /// <param name="recipe">The recipe to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The created recipe.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        Task<Recipe> CreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the detailed information of a recipe by its ID.
        /// </summary>
        /// <param name="recipeId">The unique identifier of the recipe to retrieve.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>
        /// The detailed recipe if found, or <c>null</c> if no recipe with the specified ID exists.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null or queryable source is null..</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        Task<Recipe?> GetRecipeDetailsByIdAsync(Guid recipeId, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously retrieves a list of all recipes with their associated cover image, creator, ingredients, and recipe steps.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the operation.</param>
        /// <returns>A list of recipes with detailed related information such as cover image, ingredients, and steps.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null or queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        Task<List<Recipe>> GetAllRecipesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves the newest recipes, ordered by the most recent creation date.
        /// </summary>
        /// <param name="count">The number of recipes to retrieve.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A list of the newest recipes.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null or queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Recipe>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a random selection of recipes, based on the specified count.
        /// </summary>
        /// <param name="count">The number of random recipes to retrieve. Must be greater than zero.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A list of random recipes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="count"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Recipe>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of recipes filtered by the specified cuisine.
        /// </summary>
        /// <param name="cuisine">The cuisine type to filter recipes by. Cannot be null or empty.</param>
        /// <param name="count">The number of recipes to retrieve. Must be greater than zero.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A list of recipes filtered by the specified cuisine.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cuisine"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="count"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Recipe>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a list of recipes filtered by the specified difficulty level.
        /// </summary>
        /// <param name="difficulty">The difficulty level to filter recipes by. Must match a valid value in <see cref="DifficultyEnum"/>.</param>
        /// <param name="count">The number of recipes to retrieve. Must be greater than zero.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A list of recipes filtered by the specified difficulty level.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="difficulty"/> is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if the <paramref name="difficulty"/> is not a valid value in <see cref="DifficultyEnum"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="count"/> is less than or equal to zero.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        Task<List<Recipe>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken);
        Task<List<Recipe>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken);
    }
}
