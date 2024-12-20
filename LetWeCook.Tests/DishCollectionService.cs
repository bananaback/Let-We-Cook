using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.CollectionRecipeRepositories;
using LetWeCook.Data.Repositories.DishCollectionRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DishCollectionServices;
using LetWeCook.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LetWeCook.Services.Tests
{
    public class DishCollectionServiceTests
    {
        private readonly Mock<IDishCollectionRepository> _dishCollectionRepositoryMock = new();
        private readonly Mock<ICollectionRecipeRepository> _collectionRecipeRepositoryMock = new();
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly DishCollectionService _service;

        public DishCollectionServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _service = new DishCollectionService(
                _dishCollectionRepositoryMock.Object,
                _collectionRecipeRepositoryMock.Object,
                _recipeRepositoryMock.Object,
                _userManagerMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task AddRecipeToCollectionAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var userId = "invalid-user-id";
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(
                () => _service.AddRecipeToCollectionAsync(userId, Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None)
            );
            Assert.Equal($"User with id {userId} not found.", exception.Message);
        }

        [Fact]
        public async Task AddRecipeToCollectionAsync_ShouldThrowException_WhenRecipeNotFound()
        {
            // Arrange
            var userId = "valid-user-id";
            var user = new ApplicationUser();
            var recipeId = Guid.NewGuid();
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _recipeRepositoryMock.Setup(rr => rr.GetRecipeDetailsByIdAsync(recipeId, It.IsAny<CancellationToken>())).ReturnsAsync((Recipe)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<RecipeRetrievalException>(
                () => _service.AddRecipeToCollectionAsync(userId, Guid.NewGuid(), recipeId, CancellationToken.None)
            );
            Assert.Equal($"Recipe with id {recipeId} not found.", exception.Message);
        }

        [Fact]
        public async Task AddRecipeToCollectionAsync_ShouldThrowException_WhenCollectionNotFound()
        {
            // Arrange
            var userId = "valid-user-id";
            var user = new ApplicationUser();
            var recipe = new Recipe();
            var collectionId = Guid.NewGuid();
            _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _recipeRepositoryMock.Setup(rr => rr.GetRecipeDetailsByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(recipe);
            _dishCollectionRepositoryMock.Setup(dc => dc.GetDishCollectionByIdAsync(collectionId, It.IsAny<CancellationToken>())).ReturnsAsync((DishCollection)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DishCollectionRetrievalException>(
                () => _service.AddRecipeToCollectionAsync(userId, collectionId, Guid.NewGuid(), CancellationToken.None)
            );
            Assert.Equal($"Collection with id {collectionId} not found.", exception.Message);
        }

        [Fact]
        public async Task DeleteDishCollectionAsync_ShouldReturnFalse_WhenCollectionNotFound()
        {
            // Arrange
            var collectionId = Guid.NewGuid();
            _dishCollectionRepositoryMock.Setup(dc => dc.GetDishCollectionByIdAsync(collectionId, It.IsAny<CancellationToken>())).ReturnsAsync((DishCollection)null);

            // Act
            var result = await _service.DeleteDishCollectionAsync("valid-user-id", collectionId, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }
}
