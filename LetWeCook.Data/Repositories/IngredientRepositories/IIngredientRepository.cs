using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.IngredientRepositories
{
    public interface IIngredientRepository
    {
        Task<Result<Ingredient>> CreateIngredientAsync(Ingredient ingredient, CancellationToken cancellationToken);
        Task<Result<List<Ingredient>>> GetAllIngredientsWithCoverImageAndSectionsAsync(CancellationToken cancellationToken);
        Task<Result<Ingredient>> GetInredientWithCoverImageAndSectionByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<List<Ingredient>>> GetAllIngredientsNameAndIdAsync(CancellationToken cancellationToken);
        Task<Result<List<Ingredient>>> GetIngredientsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken);
        Task<Result<List<Ingredient>>> GetIngredientsForDataExporter(CancellationToken cancellationToken);
        Task<Result<Ingredient>> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken);

    }
}
