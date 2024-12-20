using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.IngredientServices
{
    public interface IIngredientService
    {
        Task<IngredientDTO> CreateIngredientAsync(RawIngredientDTO ingredientDTO, CancellationToken cancellationToken);
        Task<PaginatedResult<IngredientDTO>> SearchIngredientsAsync(string name = "", int page = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<IngredientDTO> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<IngredientDTO>> GetIngredientsNameAndIdAsync(CancellationToken cancellationToken);
        Task DeleteIngredientByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<IngredientDTO> UpdateIngredientAsync(RawIngredientDTO ingredientDTO, CancellationToken cancellationToken);
    }
}
