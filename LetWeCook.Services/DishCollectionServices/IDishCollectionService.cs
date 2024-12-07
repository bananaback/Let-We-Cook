using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.DishCollectionServices
{
    public interface IDishCollectionService
    {
        Task<DishCollectionDTO> CreateDishCollectionAsync(string userId, DishCollectionDTO collectionDTO, CancellationToken cancellationToken);
        Task<List<DishCollectionDTO>> GetUserDishCollectionsAsync(string userId, CancellationToken cancellationToken);
        Task<CollectionRecipeDTO> AddRecipeToCollectionAsync(string userId, Guid collectionId, Guid recipeId, CancellationToken cancellationToken);
        Task<bool> DeleteDishCollectionAsync(string userId, Guid collectionId, CancellationToken cancellationToken);
        Task<DishCollectionDTO> GetCollectionDetailsByIdAsync(Guid collectionId, CancellationToken cancellationToken);
        Task<bool> RemoveRecipeFromCollection(string userId, Guid collectionId, Guid recipeId, CancellationToken cancellationToken);

    }
}
