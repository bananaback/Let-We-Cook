using LetWeCook.Data.Entities;
using LetWeCook.Services.AuthenticationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;

namespace LetWeCook.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IUrlHelperFactory> _mockUrlHelperFactory;
        private readonly Mock<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender> _mockEmailSender;
        private readonly Mock<ILogger<AuthenticationService>> _mockLogger;

        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _mockUserManager = MockUserManager();
            _mockSignInManager = MockSignInManager();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            _mockEmailSender = new Mock<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender>();
            _mockLogger = new Mock<ILogger<AuthenticationService>>();

            _authService = new AuthenticationService(
                _mockUserManager.Object,
                _mockHttpContextAccessor.Object,
                _mockUrlHelperFactory.Object,
                _mockEmailSender.Object,
                _mockSignInManager.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFailed_WhenEmailIsDuplicate()
        {
            // Arrange
            string email = "test@example.com";
            _mockUserManager.Setup(x => x.FindByEmailAsync(email))
                .ReturnsAsync(new ApplicationUser());

            // Act
            var result = await _authService.RegisterUserAsync("username", email, "password");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Code == "DuplicateEmail");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFailed_WhenUsernameIsDuplicate()
        {
            // Arrange
            string username = "testuser";
            _mockUserManager.Setup(x => x.FindByNameAsync(username))
                .ReturnsAsync(new ApplicationUser());

            // Act
            var result = await _authService.RegisterUserAsync(username, "test@example.com", "password");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Code == "DuplicateUsername");
        }

        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<SignInManager<ApplicationUser>> MockSignInManager()
        {
            var userManager = MockUserManager();
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            return new Mock<SignInManager<ApplicationUser>>(
                userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        }

        private static HttpContext MockHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Request.Scheme = "https";
            return context;
        }
    }
}