using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.RecipeRepositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly LetWeCookDbContext _context;
        public RecipeRepository(LetWeCookDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Asynchronously creates a new recipe in the database.
        /// </summary>
        /// <param name="recipe">The recipe to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The created recipe.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        public async Task<Recipe> CreateRecipeAsync(Recipe recipe, CancellationToken cancellationToken = default)
        {
            // Add the recipe to the Recipes DbSet asynchronously
            await _context.Recipes.AddAsync(recipe, cancellationToken);

            // Return the created recipe. The actual save to the database should be done in a later step (possibly in a Unit of Work or Commit method).
            return recipe;
        }


        /// <summary>
        /// Asynchronously retrieves a list of recipe overviews for a specific user by their user ID.
        /// </summary>
        /// <param name="userId">The user ID whose recipes are to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A list of recipes associated with the specified user.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="userId"/> is null or queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        public async Task<List<Recipe>> GetRecipeOverviewByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Validate the userId to prevent passing a null GUID
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            // Asynchronously fetch recipes created by the specified user, including their cover image and creator information
            List<Recipe> recipes = await _context.Recipes
                .Include(r => r.RecipeCoverImage)  // Eagerly load the RecipeCoverImage navigation property
                .Include(r => r.CreatedBy)        // Eagerly load the CreatedBy navigation property
                .Where(r => r.CreatedBy.Id == userId)  // Filter by the user ID
                .ToListAsync(cancellationToken);  // Execute the query asynchronously with cancellation support

            return recipes;
        }

        /// <summary>
        /// Asynchronously retrieves a list of all recipes with their associated cover image, creator, ingredients, and recipe steps.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the operation.</param>
        /// <returns>A list of recipes with detailed related information such as cover image, ingredients, and steps.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null or queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the cancellation token.</exception>
        public async Task<List<Recipe>> GetAllRecipesAsync(CancellationToken cancellationToken = default)
        {
            // Use AsSplitQuery() to split the query into multiple queries, one for each navigation property
            List<Recipe> recipes = await _context.Recipes
                .Include(r => r.RecipeCoverImage)  // Eagerly load the RecipeCoverImage navigation property
                .Include(r => r.CreatedBy)        // Eagerly load the CreatedBy navigation property (creator of the recipe)
                .Include(r => r.RecipeIngredients)  // Eagerly load RecipeIngredients navigation property
                    .ThenInclude(ri => ri.Ingredient)  // Eagerly load the related Ingredient for each RecipeIngredient
                .Include(r => r.RecipeSteps)       // Eagerly load RecipeSteps navigation property
                    .ThenInclude(rs => rs.RecipeStepMedias)  // Eagerly load RecipeStepMedias for each RecipeStep
                .AsSplitQuery()                    // Automatically splits the query into multiple SQL queries
                .ToListAsync(cancellationToken);   // Execute the query asynchronously with cancellation support

            return recipes;
        }



        /// <summary>
        /// Retrieves the newest recipes, ordered by the most recent creation date.
        /// </summary>
        /// <param name="count">The number of recipes to retrieve.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A list of the newest recipes.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null or queryable source is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<Recipe>> GetNewestRecipesAsync(int count, CancellationToken cancellationToken = default)
        {
            // Validate count to prevent invalid inputs
            if (count <= 0)
            {
                throw new ArgumentNullException(nameof(count), "Count must be greater than zero.");
            }

            // Query the recipes sorted by the most recent creation date
            List<Recipe> newestRecipes = await _context.Recipes
                .Include(r => r.RecipeCoverImage)  // Eagerly load the RecipeCoverImage navigation property
                .OrderByDescending(r => r.DateCreated) // Sort by creation date in descending order
                .Take(count) // Limit the number of results to `count`
                .ToListAsync(cancellationToken);  // Execute the query asynchronously with cancellation support

            return newestRecipes;
        }

        /// <summary>
        /// Retrieves a random selection of recipes, based on the specified count.
        /// </summary>
        /// <param name="count">The number of random recipes to retrieve. Must be greater than zero.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        /// <returns>A list of random recipes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the <paramref name="count"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="cancellationToken"/> is null.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<Recipe>> GetRandomRecipesAsync(int count, CancellationToken cancellationToken = default)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            // Fetch a random selection of recipes using GUID for randomness
            List<Recipe> randomRecipes = await _context.Recipes
                .Include(r => r.RecipeCoverImage)  // Eagerly load the RecipeCoverImage navigation property
                .OrderBy(r => Guid.NewGuid()) // Randomize order using GUID
                .Take(count) // Take the specified number of random recipes
                .ToListAsync(cancellationToken); // Execute the query asynchronously with cancellation support

            return randomRecipes;
        }

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
        public async Task<List<Recipe>> GetRecipesByCuisineAsync(string cuisine, int count, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(cuisine))
            {
                throw new ArgumentNullException(nameof(cuisine), "Cuisine cannot be null or empty.");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            // Fetch recipes filtered by cuisine
            List<Recipe> recipes = await _context.Recipes
                .Where(r => r.Cuisine == cuisine) // Filter by cuisine
                .Include(r => r.RecipeCoverImage)
                .Take(count) // Limit to the specified count
                .ToListAsync(cancellationToken); // Execute the query asynchronously with cancellation support

            return recipes;
        }



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
        public async Task<List<Recipe>> GetRecipesByDifficultyAsync(string difficulty, int count, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(difficulty))
            {
                throw new ArgumentNullException(nameof(difficulty), "Difficulty cannot be null or empty.");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            // Parse the difficulty into an enum; throws an exception if invalid
            var parsedDifficulty = Enum.Parse<DifficultyEnum>(difficulty, true);

            // Fetch recipes filtered by difficulty
            List<Recipe> recipes = await _context.Recipes
                .Where(r => r.Difficulty == parsedDifficulty) // Filter by difficulty
                .Include(r => r.RecipeCoverImage) // Eagerly load the RecipeCoverImage navigation property
                .Include(r => r.CreatedBy) // Eagerly load the CreatedBy navigation property
                .Include(r => r.RecipeIngredients) // Eagerly load RecipeIngredients navigation property
                    .ThenInclude(ri => ri.Ingredient) // Eagerly load the related Ingredient for each RecipeIngredient
                .Include(r => r.RecipeSteps) // Eagerly load RecipeSteps navigation property
                    .ThenInclude(rs => rs.RecipeStepMedias) // Eagerly load RecipeStepMedias for each RecipeStep
                .OrderByDescending(r => r.DateCreated) // Sort by newest
                .Take(count) // Limit to the specified count
                .ToListAsync(cancellationToken); // Execute the query asynchronously with cancellation support

            return recipes;
        }


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
        public async Task<Recipe?> GetRecipeDetailsByIdAsync(Guid recipeId, CancellationToken cancellationToken = default)
        {
            // Check for cancellation
            cancellationToken.ThrowIfCancellationRequested();

            // Retrieve the recipe by ID, including related navigation properties
            Recipe? recipe = await _context.Recipes
                .Include(r => r.RecipeCoverImage) // Eagerly load the RecipeCoverImage navigation property
                .Include(r => r.CreatedBy) // Eagerly load the CreatedBy navigation property
                .Include(r => r.RecipeIngredients) // Eagerly load RecipeIngredients navigation property
                    .ThenInclude(ri => ri.Ingredient) // Eagerly load the related Ingredient for each RecipeIngredient
                .Include(r => r.RecipeSteps) // Eagerly load RecipeSteps navigation property
                    .ThenInclude(rs => rs.RecipeStepMedias) // Eagerly load RecipeStepMedias for each RecipeStep
                .Where(r => r.Id == recipeId) // Filter by recipe ID
                .FirstOrDefaultAsync(cancellationToken); // Execute the query asynchronously with cancellation support

            // Return the recipe if found, or null if not
            return recipe;
        }


        public Task<List<Recipe>> GetTrendingRecipesAsync(int count, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
