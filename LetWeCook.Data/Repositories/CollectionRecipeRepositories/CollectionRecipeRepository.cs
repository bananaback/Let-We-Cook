using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.CollectionRecipeRepositories
{
    public class CollectionRecipeRepository : ICollectionRecipeRepository
    {
        private readonly LetWeCookDbContext _context;
        public CollectionRecipeRepository(LetWeCookDbContext context)
        {
            _context = context;
        }
        public async Task<CollectionRecipe> CreateCollectionRecipeAsync(CollectionRecipe collectionRecipe, CancellationToken cancellationToken)
        {
            await _context.CollectionRecipes.AddAsync(collectionRecipe, cancellationToken);
            return collectionRecipe;
        }

        public async Task DeleteCollectionRecipeAsync(Guid collectionId, Guid recipeId, CancellationToken cancellationToken)
        {
            var recipe = await _context.CollectionRecipes.Where(cr => cr.Recipe.Id == recipeId && cr.Collection.Id == collectionId)
                .FirstOrDefaultAsync();
            if (recipe == null)
            {
                return;
            }
            _context.CollectionRecipes.Remove(recipe);
        }
    }
}
