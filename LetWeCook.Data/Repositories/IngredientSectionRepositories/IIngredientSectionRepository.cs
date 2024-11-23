using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.IngredientSectionRepositories
{
    public interface IIngredientSectionRepository
    {
        Task<Result<IngredientSection>> GetIngredientSectionByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
