using LetWeCook.Data.Entities;
using LetWeCook.Data.Enums;
using LetWeCook.Data.Repositories.ProfileRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.ProfileServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace LetWeCook.Tests.Services
{
    public class ProfileServiceTests
    {
        private readonly Mock<IProfileRepository> _mockProfileRepository;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<ProfileService>> _mockLogger;
        private readonly ProfileService _profileService;

        public ProfileServiceTests()
        {
            _mockProfileRepository = new Mock<IProfileRepository>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null
            );
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<ProfileService>>();

            _profileService = new ProfileService(
                _mockProfileRepository.Object,
                _mockUserManager.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnProfile_WhenProfileExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userIdString = userId.ToString();

            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                User = new ApplicationUser
                {
                    Id = userId,
                    UserName = "testuser",
                    Email = "testuser@example.com",
                    DateJoined = DateTime.UtcNow
                },
                PhoneNumber = "123456789",
                FirstName = "Test",
                LastName = "User",
                Age = 25,
                Gender = GenderEnum.MALE,
                Address = "123 Test St"
            };

            _mockProfileRepository
                .Setup(repo => repo.GetUserProfileByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(userProfile);

            _mockUserManager
                .Setup(um => um.FindByIdAsync(userIdString))
                .ReturnsAsync(userProfile.User);

            _mockUserManager
                .Setup(um => um.GetClaimsAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<System.Security.Claims.Claim>());

            // Act
            var result = await _profileService.GetUserProfileAsync(userIdString);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userProfile.Id, result.Id);
            Assert.Equal(userProfile.User.Id, result.UserId);
            Assert.Equal(userProfile.User.UserName, result.UserName);
            Assert.Equal(userProfile.User.Email, result.Email);
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldThrowException_WhenUserIdIsInvalid()
        {
            // Arrange
            var invalidUserId = "invalid-guid";

            // Act & Assert
            await Assert.ThrowsAsync<UserProfileRetrievalException>(() =>
                _profileService.GetUserProfileAsync(invalidUserId));
        }

        [Fact]
        public async Task UpdateUserProfileAsync_ShouldThrowException_WhenProfileDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var profileDto = new ProfileDTO { UserId = userId };

            _mockProfileRepository
                .Setup(repo => repo.GetUserProfileByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserProfile)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserProfileRetrievalException>(() =>
                _profileService.UpdateUserProfileAsync(profileDto));
        }
    }
}
