using LetWeCook.Services.DTOs;
using LetWeCook.Data.Repositories.DietaryPreferenceRepositories;
using LetWeCook.Data.Repositories.UserDietaryPreferenceRepositories;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.ProfileRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace LetWeCook.Services.UserDietaryPreferenceServices
{
    public class UserDietaryPreferenceService : IUserDietaryPreferenceService
    {
        private readonly IDietaryPreferenceRepository _dietaryPreferenceRepository;
        private readonly IUserDietaryPreferenceRepository _userDietaryPreferenceRepository;
        private readonly IProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork; // Inject UnitOfWork
        private readonly ILogger<UserDietaryPreferenceService> _logger;

        public UserDietaryPreferenceService(
            IDietaryPreferenceRepository dietaryPreferenceRepository,
            IUserDietaryPreferenceRepository userDietaryPreferenceRepository,
            IProfileRepository userProfileRepository,
            IUnitOfWork unitOfWork,
            ILogger<UserDietaryPreferenceService> logger) // Add UnitOfWork parameter
        {
            _dietaryPreferenceRepository = dietaryPreferenceRepository;
            _userDietaryPreferenceRepository = userDietaryPreferenceRepository;
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork; // Assign instance
            _logger = logger;
        }

        public async Task<List<DietaryPreferenceDTO>> GetAllDietaryPreferences(CancellationToken cancellationToken)
        {
            var dietaryPreferences = await _dietaryPreferenceRepository.GetAllAsync(cancellationToken);
            return dietaryPreferences.Select(dp => new DietaryPreferenceDTO
            {
                Id = dp.Id,
                Value = dp.Value,
                Description = dp.Description,
                Color = dp.Color,
                Icon = dp.Icon
            }).ToList();
        }

        public async Task SaveDietaryPreferencesAsync(Guid userId, SaveDietaryPreferencesDTO dto, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saving dietary preferences for User ID: {UserId}", userId);

            if (dto == null || dto.Preferences == null || dto.Preferences.Count == 0)
            {
                _logger.LogWarning("No dietary preferences provided for User ID: {UserId}", userId);
                return;
            }

            foreach (var preferenceDto in dto.Preferences)
            {
                _logger.LogInformation("Preference - Value: {Value}, Description: {Description}, Color: {Color}, Icon: {Icon}, IsSelected: {IsSelected}",
                    preferenceDto.Value, preferenceDto.Description, preferenceDto.Color, preferenceDto.Icon, preferenceDto.IsSelected);
            }

            var userProfile = await _userProfileRepository.GetUserProfileByUserIdAsync(userId, cancellationToken);
            if (userProfile == null)
            {
                _logger.LogError("User profile not found for User ID: {UserId}", userId);
                throw new InvalidOperationException("User not found.");
            }

            var existingUserPreferences = await _userDietaryPreferenceRepository.GetAllByUserIdAsync(userId, cancellationToken);
            var existingPreferenceIds = existingUserPreferences
                .Select(up => up.DietaryPreference.Id)
                .ToHashSet();

            foreach (var preferenceDto in dto.Preferences)
            {
                var preferenceEntity = await _dietaryPreferenceRepository.GetByValueAsync(preferenceDto.Value, cancellationToken);
                if (preferenceEntity == null)
                {
                    _logger.LogWarning("Dietary preference not found for Value: {Value}", preferenceDto.Value);
                    continue;
                }

                if (preferenceDto.IsSelected)
                {
                    if (!existingPreferenceIds.Contains(preferenceEntity.Id))
                    {
                        _logger.LogInformation("Adding new dietary preference: {Value} for User ID: {UserId}", preferenceDto.Value, userId);

                        var newUserDietaryPreference = new UserDietaryPreference
                        {
                            UserProfile = userProfile,
                            DietaryPreference = preferenceEntity
                        };

                        await _userDietaryPreferenceRepository.AddAsync(newUserDietaryPreference, cancellationToken);
                    }
                    else
                    {
                        _logger.LogInformation("Preference already exists: {Value} for User ID: {UserId}", preferenceDto.Value, userId);
                    }
                }
                else
                {
                    var existingPreference = existingUserPreferences
                        .FirstOrDefault(up => up.DietaryPreference.Id == preferenceEntity.Id);

                    if (existingPreference != null)
                    {
                        _logger.LogInformation("Removing dietary preference: {Value} for User ID: {UserId}", preferenceDto.Value, userId);
                        await _userDietaryPreferenceRepository.RemoveAsync(existingPreference, cancellationToken);
                    }
                }
            }

            _logger.LogInformation("Saving changes to database for User ID: {UserId}", userId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Dietary preferences saved successfully for User ID: {UserId}", userId);
        }

    }
}
