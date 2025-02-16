using LetWeCook.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LetWeCook.Data.Repositories.UserDietaryPreferenceRepositories
{
    public interface IUserDietaryPreferenceRepository
    {
        Task<List<UserDietaryPreference>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(UserDietaryPreference userDietaryPreference, CancellationToken cancellationToken = default);
        Task RemoveAsync(UserDietaryPreference userDietaryPreference, CancellationToken cancellationToken = default);
    }
}
