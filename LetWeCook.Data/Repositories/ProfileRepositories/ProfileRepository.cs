using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetWeCook.Data.Repositories.ProfileRepositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly LetWeCookDbContext _context;
        private readonly ILogger<ProfileRepository> _logger;
        public ProfileRepository(LetWeCookDbContext context, ILogger<ProfileRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<UserProfile>> CreateUserProfile(UserProfile profile, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.UserProfiles.AddAsync(profile, cancellationToken);

                return Result<UserProfile>.Success(profile, "User profile created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Result<UserProfile>.Failure("An error occurred while creating the user profile.", ErrorCode.UserProfileCreationFailed, ex);
            }
        }

        public async Task<Result<UserProfile>> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {

            try
            {
                var profile = await _context.UserProfiles
                                .Include(up => up.User)
                                .Where(up => up.User.Id == userId)
                                .FirstOrDefaultAsync();

                if (profile == null)
                {
                    return Result<UserProfile>.Failure(
                        $"Failed to retrieve user profile with user id {userId}",
                        ErrorCode.UserProfileNotFound);
                }

                return Result<UserProfile>.Success(profile, "Retrieve user profile successfully.");
            }
            catch (Exception ex)
            {
                return Result<UserProfile>.Failure(
                    "Retrieve user profile failed",
                    ErrorCode.UserProfileRetrievalFailed,
                    ex);
            }
        }

        public void UpdateUserProfile(UserProfile profile)
        {
            _context.UserProfiles.Update(profile);
        }
    }
}
