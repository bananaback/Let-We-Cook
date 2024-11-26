using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.IngredientRepositories
{
    public interface IIngredientRepository
    {
        Task<Ingredient> CreateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken);
        Task<List<Ingredient>> GetIngredientsWithDetailsAsync(CancellationToken cancellationToken);
        Task<Ingredient?> GetIngredientWithDetailsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Ingredient>> GetAllIngredientIdsAndNamesAsync(CancellationToken cancellationToken);
        Task<List<Ingredient>> GetIngredientsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
        Task<List<Ingredient>> GetIngredientsForExportAsync(CancellationToken cancellationToken);
        Task<Ingredient?> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken);

    }
}
