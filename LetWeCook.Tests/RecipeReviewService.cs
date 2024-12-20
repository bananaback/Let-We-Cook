using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.RecipeReviewRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.RecipeReviewServices;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LetWeCook.Services.Tests
{
    public class RecipeReviewServiceTests
    {
        private readonly Mock<IRecipeReviewRepository> _recipeReviewRepositoryMock;
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly RecipeReviewService _service;

        public RecipeReviewServiceTests()
        {
            _recipeReviewRepositoryMock = new Mock<IRecipeReviewRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null
            );

            _service = new RecipeReviewService(
                _recipeReviewRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _userManagerMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task CreateReviewForUser_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var recipeId = Guid.NewGuid();
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.CreateReviewForUser(userId, recipeId, "Great recipe!", 5m));
        }

        [Fact]
        public async Task CreateReviewForUser_ShouldThrowRecipeReviewCreationException_WhenRecipeDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var recipeId = Guid.NewGuid();
            var user = new ApplicationUser { Id = Guid.Parse(userId) };

            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _recipeRepositoryMock.Setup(rr => rr.GetRecipeDetailsByIdAsync(recipeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Recipe)null);

            // Act & Assert
            await Assert.ThrowsAsync<RecipeReviewCreationException>(() => _service.CreateReviewForUser(userId, recipeId, "Great recipe!", 5m));
        }

        [Fact]
        public async Task DeleteReviewForUser_ShouldReturnTrue_WhenReviewIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var reviewId = Guid.NewGuid();
            var user = new ApplicationUser { Id = Guid.Parse(userId) };
            var review = new RecipeReview { Id = reviewId, User = user };

            _recipeReviewRepositoryMock.Setup(rr => rr.GetReviewByIdAsync(reviewId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(review);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _service.DeleteReviewForUser(userId, reviewId, It.IsAny<CancellationToken>());

            // Assert
            Assert.True(result);
            _recipeReviewRepositoryMock.Verify(rr => rr.DeleteReviewAsync(reviewId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetReviewsForRecipe_ShouldReturnReviews_WhenRecipeExists()
        {
            // Arrange
            var recipeId = Guid.NewGuid();
            var recipe = new Recipe { Id = recipeId };
            var reviews = new List<RecipeReview>
            {
                new RecipeReview { Id = Guid.NewGuid(), User = new ApplicationUser { UserName = "user1" }, Review = "Great!", Rating = 5m },
                new RecipeReview { Id = Guid.NewGuid(), User = new ApplicationUser { UserName = "user2" }, Review = "Good!", Rating = 4m }
            };

            _recipeRepositoryMock.Setup(rr => rr.GetRecipeDetailsByIdAsync(recipeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(recipe);
            _recipeReviewRepositoryMock.Setup(rr => rr.GetAllReviewsByRecipeIdAsync(recipeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(reviews);

            // Act
            var result = await _service.GetReviewsForRecipe(recipeId, CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.Username == "user1" && r.Review == "Great!" && r.Rating == 5m);
            Assert.Contains(result, r => r.Username == "user2" && r.Review == "Good!" && r.Rating == 4m);
        }
    }
}
