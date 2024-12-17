using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using LetWeCook.Data.Repositories.ProfileRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
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

        public async Task<ProfileDTO> GetUserProfileAsync(string userIdString, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                throw new UserProfileRetrievalException($"Invalid user id guid: {userIdString}");

            }

            try
            {
                var userProfile = await _profileRepository.GetUserProfileByUserIdAsync(userId, cancellationToken);

                if (userProfile == null)
                {
                    var user = await _userManager.FindByIdAsync(userIdString);

                    if (user == null)
                    {
                        throw new UserProfileRetrievalException($"User with id {userId} not found");
                    }

                    userProfile = new UserProfile
                    {
                        Id = Guid.NewGuid(),
                        User = user
                    };

                    await _profileRepository.CreateUserProfile(userProfile, cancellationToken);

                    var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                    if (saveChangesResult <= 0) // Check if no changes were saved
                    {
                        throw new UserProfileCreationException("Failed to save changes to the database after user profile creation.");
                    }

                    return new ProfileDTO
                    {
                        Id = userProfile.Id,
                        UserId = userId,
                        UserName = user.UserName ?? string.Empty,
                    };
                }

                var currUser = await _userManager.FindByIdAsync(userIdString);
                var currentClaims = await _userManager.GetClaimsAsync(currUser!);
                var existingPictureClaim = currentClaims.FirstOrDefault(c => c.Type == "picture");
                string avatarUrl = existingPictureClaim?.Value ?? "https://th.bing.com/th/id/OIP.6UhgwprABi3-dz8Qs85FvwHaHa?rs=1&pid=ImgDetMain";
                ;



                return new ProfileDTO
                {
                    Id = userProfile.Id,
                    AvatarUrl = avatarUrl,
                    UserId = userId,
                    UserName = userProfile.User.UserName ?? string.Empty,
                    DateJoined = userProfile.User.DateJoined,
                    Email = userProfile.User.Email ?? string.Empty,
                    PhoneNumber = userProfile.PhoneNumber,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    Age = userProfile.Age,
                    Gender = userProfile.Gender.ToString(),
                    Address = userProfile.Address
                };

            }

            catch (ArgumentNullException ex)
            {
                throw new UserProfileRetrievalException("Failed to retrie user profile", ex);
            }
        }

        public async Task<ProfileDTO> UpdateUserProfileAsync(ProfileDTO profileDTO, CancellationToken cancellationToken = default)
        {

            var oldProfile = await _profileRepository.GetUserProfileByUserIdAsync(profileDTO.UserId, cancellationToken);
            if (oldProfile == null)
            {
                throw new UserProfileRetrievalException($"Failed to retrieve user profile with user id {profileDTO.UserId.ToString()}");
            }

            oldProfile.PhoneNumber = profileDTO.PhoneNumber;
            oldProfile.FirstName = profileDTO.FirstName;
            oldProfile.LastName = profileDTO.LastName;
            oldProfile.Age = profileDTO.Age;
            oldProfile.Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), profileDTO.Gender);
            oldProfile.Address = profileDTO.Address;

            await _profileRepository.UpdateUserProfile(oldProfile);

            var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (saveChangesResult <= 0) // Check if no changes were saved
            {
                throw new UserProfileCreationException("Failed to save changes to the database after profile update.");
            }

            return profileDTO;
        }
    }
}
