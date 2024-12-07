using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.DishCollectionRepositories
{
    public class DishCollectionRepository : IDishCollectionRepository
    {
        private readonly LetWeCookDbContext _context;
        public DishCollectionRepository(LetWeCookDbContext context)
        {
            _context = context;
        }
        public async Task<DishCollection> CreateDishCollectionAsync(DishCollection dishCollection, CancellationToken cancellationToken)
        {
            await _context.DishCollections.AddAsync(dishCollection, cancellationToken);
            return dishCollection;
        }

        public async Task<bool> DeleteDishCollectionAsync(Guid collectionId, CancellationToken cancellationToken)
        {
            var collection = await _context.DishCollections.Where(dc => dc.Id == collectionId).FirstOrDefaultAsync();
            if (collection == null) { return false; }
            _context.DishCollections.Remove(collection);
            return true;
        }

        public Task<List<DishCollection>> GetAllDishCollectionsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return _context.DishCollections
                .Include(dc => dc.CollectionRecipes)
                .ThenInclude(cr => cr.Recipe)
                .ThenInclude(r => r.RecipeCoverImage)
                .Where(dc => dc.User.Id == userId)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);
        }

        public async Task<DishCollection?> GetDishCollectionByIdAsync(Guid collectionId, CancellationToken cancellationToken)
        {
            return await _context.DishCollections
                .Include(dc => dc.User)
                .FirstOrDefaultAsync(dc => dc.Id == collectionId, cancellationToken);
        }

        public async Task<DishCollection?> GetDishCollectionDetailsByIdAsync(Guid collectionId, CancellationToken cancellationToken)
        {
            return await _context.DishCollections
                .Include(dc => dc.Recipes)
                .ThenInclude(r => r.RecipeCoverImage)
                .FirstOrDefaultAsync(dc => dc.Id == collectionId, cancellationToken);
        }
    }
}
