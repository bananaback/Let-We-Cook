using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.UserDietaryPreferenceServices
{
    public interface IUserDietaryPreferenceService
    {
        Task<List<DietaryPreferenceDTO>> GetAllDietaryPreferences(CancellationToken cancellationToken);
        Task SaveDietaryPreferencesAsync(Guid userId, SaveDietaryPreferencesDTO dto, CancellationToken cancellationToken);

    }
}
