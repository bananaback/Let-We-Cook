using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.UserDietaryPreferenceRepositories
{
    public class UserDietaryPreferenceRepository : IUserDietaryPreferenceRepository
    {
        private readonly LetWeCookDbContext _context;

        public UserDietaryPreferenceRepository(LetWeCookDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDietaryPreference>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserDietaryPreferences
                .Where(udp => udp.UserProfile.User.Id == userId) // Match user ID
                .Include(udp => udp.DietaryPreference) // Ensure DietaryPreference is loaded
                .Include(udp => udp.UserProfile) // Ensure UserProfile is loaded
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(UserDietaryPreference userDietaryPreference, CancellationToken cancellationToken = default)
        {
            _context.UserDietaryPreferences.Add(userDietaryPreference);
        }

        public async Task RemoveAsync(UserDietaryPreference userDietaryPreference, CancellationToken cancellationToken = default)
        {
            _context.UserDietaryPreferences.Remove(userDietaryPreference);
        }
    }
}
