using LetWeCook.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LetWeCook.Data.Repositories.DietaryPreferenceRepositories
{
    public interface IDietaryPreferenceRepository
    {
        Task<DietaryPreference> CreateAsync(DietaryPreference dietaryPreference, CancellationToken cancellationToken = default);
        Task<DietaryPreference?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<DietaryPreference>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<DietaryPreference> UpdateAsync(DietaryPreference dietaryPreference, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        // New method to get a dietary preference by its value
        Task<DietaryPreference?> GetByValueAsync(string value, CancellationToken cancellationToken = default);
    }
}
