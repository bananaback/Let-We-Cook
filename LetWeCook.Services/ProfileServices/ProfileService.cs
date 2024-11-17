using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using LetWeCook.Data.Repositories.ProfileRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LetWeCook.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProfileService> _logger;
        public ProfileService(IProfileRepository profileRepository,
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            ILogger<ProfileService> logger)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<ProfileDTO>> GetUserProfileAsync(string userIdString, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return Result<ProfileDTO>.Failure("Invalid user id guid", ErrorCode.UserNotFound);

            }
            var getUserProfileResult = await _profileRepository.GetUserProfileByUserIdAsync(userId, cancellationToken);

            if (!getUserProfileResult.IsSuccess)
            {
                if (getUserProfileResult.ErrorCode == ErrorCode.UserProfileNotFound)
                {
                    var user = await _userManager.FindByIdAsync(userIdString);

                    if (user == null)
                    {
                        return Result<ProfileDTO>.Failure($"User with id {userId} not found", ErrorCode.UserNotFound);
                    }

                    var userProfile = new UserProfile
                    {
                        Id = Guid.NewGuid(),
                        User = user
                    };

                    var createUserProfileResult = await _profileRepository.CreateUserProfile(userProfile, cancellationToken);

                    if (!createUserProfileResult.IsSuccess)
                    {
                        return Result<ProfileDTO>.Failure("Failed to create user profile", ErrorCode.UserProfileCreationFailed, createUserProfileResult.Exception);
                    }

                    var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                    if (saveChangesResult <= 0) // Check if no changes were saved
                    {
                        return Result<ProfileDTO>.Failure("Failed to save changes to the database after user profile creation.", ErrorCode.DatabaseSaveFailed);
                    }

                    return Result<ProfileDTO>.Success(new ProfileDTO
                    {
                        Id = userProfile.Id,
                        UserId = userId,
                        UserName = user.UserName ?? string.Empty,
                    }, "Create user profile successfully.");
                }
                else
                {
                    return Result<ProfileDTO>.Failure("Failed to get user profile", ErrorCode.UserProfileRetrievalFailed, getUserProfileResult.Exception);
                }
            }

            var existingProfile = getUserProfileResult.Data!;

            return Result<ProfileDTO>.Success(new ProfileDTO
            {
                Id = existingProfile.Id,
                UserId = userId,
                UserName = existingProfile.User.UserName ?? string.Empty,
                DateJoined = existingProfile.User.DateJoined,
                Email = existingProfile.User.Email ?? string.Empty,
                PhoneNumber = existingProfile.PhoneNumber,
                FirstName = existingProfile.FirstName,
                LastName = existingProfile.LastName,
                Age = existingProfile.Age,
                Address = existingProfile.Address
            }, "Retrieve user profile successfully.");
        }

        public async Task<Result<ProfileDTO>> UpdateUserProfileAsync(ProfileDTO profileDTO, CancellationToken cancellationToken = default)
        {

            var getExistingUserProfileResult = await _profileRepository.GetUserProfileByUserIdAsync(profileDTO.UserId, cancellationToken);
            if (!getExistingUserProfileResult.IsSuccess)
            {
                return Result<ProfileDTO>.Failure($"Failed to retrieve user profile with user id {profileDTO.UserId.ToString()}", ErrorCode.UserProfileRetrievalFailed, getExistingUserProfileResult.Exception);
            }

            var oldProfile = getExistingUserProfileResult.Data!;

            oldProfile.PhoneNumber = profileDTO.PhoneNumber;
            oldProfile.FirstName = profileDTO.FirstName;
            oldProfile.LastName = profileDTO.LastName;
            oldProfile.Age = profileDTO.Age;
            oldProfile.Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), profileDTO.Gender);
            oldProfile.Address = profileDTO.Address;

            _profileRepository.UpdateUserProfile(oldProfile);

            var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (saveChangesResult <= 0) // Check if no changes were saved
            {
                return Result<ProfileDTO>.Failure("Failed to save changes to the database after profile update.", ErrorCode.UserProfileSaveFailed);
            }

            return Result<ProfileDTO>.Success(profileDTO, "Updated user profile successfully.");
        }
    }
}
