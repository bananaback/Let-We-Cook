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


        public async Task<UserProfile> CreateUserProfile(UserProfile profile, CancellationToken cancellationToken = default)
        {
            await _context.UserProfiles.AddAsync(profile, cancellationToken);

            return profile;
        }



        public async Task<UserProfile?> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");
            }

            var profile = await _context.UserProfiles
                            .Include(up => up.User)
                            .Where(up => up.User.Id == userId)
                            .FirstOrDefaultAsync(cancellationToken); // Added cancellationToken

            return profile;
        }


        public async Task UpdateUserProfile(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile), "Profile cannot be null.");
            }

            _context.UserProfiles.Update(profile);

            await Task.CompletedTask; // Not truly asynchronous; you may want to save changes to the database instead
        }

    }
}
