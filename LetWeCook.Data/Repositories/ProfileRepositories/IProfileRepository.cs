using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.ProfileRepositories
{
    public interface IProfileRepository
    {
        Task<UserProfile?> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserProfile> CreateUserProfile(UserProfile profile, CancellationToken cancellationToken);
        Task UpdateUserProfile(UserProfile profile);
    }
}
