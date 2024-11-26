using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.IngredientSectionRepositories
{
    public interface IIngredientSectionRepository
    {
        Task<IngredientSection?> GetIngredientSectionByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
