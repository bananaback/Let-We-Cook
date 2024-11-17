using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.ProfileRepositories
{
    public interface IProfileRepository
    {
        Task<Result<UserProfile>> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<UserProfile>> CreateUserProfile(UserProfile profile, CancellationToken cancellationToken);
        void UpdateUserProfile(UserProfile profile);
    }
}
