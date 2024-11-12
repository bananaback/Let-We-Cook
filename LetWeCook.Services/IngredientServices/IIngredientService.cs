using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.IngredientServices
{
    public interface IIngredientService
    {
        Task<Result<IngredientDTO>> CreateIngredientAsync(RawIngredientDTO ingredientDTO, CancellationToken cancellationToken);
        Task<Result<PaginatedResult<IngredientDTO>>> SearchIngredientsAsync(string name = "", int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<Result<IngredientDTO>> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<List<IngredientDTO>>> GetIngredientsNameAndIdAsync(CancellationToken cancellationToken);
    }
}
