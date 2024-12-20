using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.IngredientSectionRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.FileStorageServices;
using LetWeCook.Services.IngredientServices;
using Moq;

namespace LetWeCook.Tests.Services
{
    public class IngredientServiceTests
    {
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
        private readonly Mock<IMediaUrlRepository> _mediaUrlRepositoryMock;
        private readonly Mock<IIngredientSectionRepository> _ingredientSectionRepositoryMock;
        private readonly Mock<IFileStorageService> _fileStorageServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IngredientService _ingredientService;

        public IngredientServiceTests()
        {
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _mediaUrlRepositoryMock = new Mock<IMediaUrlRepository>();
            _ingredientSectionRepositoryMock = new Mock<IIngredientSectionRepository>();
            _fileStorageServiceMock = new Mock<IFileStorageService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _ingredientService = new IngredientService(
                _ingredientRepositoryMock.Object,
                _mediaUrlRepositoryMock.Object,
                _ingredientSectionRepositoryMock.Object,
                _fileStorageServiceMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetIngredientByIdAsync_ShouldReturnIngredient()
        {
            // Arrange
            var ingredientId = Guid.NewGuid();
            var ingredient = new Ingredient
            {
                Id = ingredientId,
                Name = "Tomato",
                Description = "Fresh tomato",
                CoverImageUrl = new MediaUrl { Url = "http://example.com/tomato.jpg" },
                IngredientSections = new List<IngredientSection>
                {
                    new IngredientSection { Id = Guid.NewGuid(), TextContent = "Rich in vitamins", Order = 1 },
                    new IngredientSection { Id = Guid.NewGuid(), MediaUrl = new MediaUrl { Url = "http://example.com/vitamins.jpg" }, Order = 2 }
                }
            };

            _ingredientRepositoryMock.Setup(repo => repo.GetIngredientWithDetailsByIdAsync(ingredientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredient);

            // Act
            var result = await _ingredientService.GetIngredientByIdAsync(ingredientId, It.IsAny<CancellationToken>());

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Tomato", result.IngredientName);
            Assert.Equal(2, result.Frames.Count);
            _ingredientRepositoryMock.Verify(repo => repo.GetIngredientWithDetailsByIdAsync(ingredientId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteIngredientByIdAsync_ShouldDeleteIngredientSuccessfully()
        {
            // Arrange
            var ingredientId = Guid.NewGuid();
            var ingredient = new Ingredient { Id = ingredientId };

            _ingredientRepositoryMock.Setup(repo => repo.GetIngredientByIdAsync(ingredientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredient);

            _ingredientRepositoryMock.Setup(repo => repo.DeleteIngredientAsync(ingredient))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            await _ingredientService.DeleteIngredientByIdAsync(ingredientId);

            // Assert
            _ingredientRepositoryMock.Verify(repo => repo.DeleteIngredientAsync(ingredient), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteIngredientByIdAsync_ShouldThrowException_WhenIngredientNotFound()
        {
            // Arrange
            var ingredientId = Guid.NewGuid();

            _ingredientRepositoryMock.Setup(repo => repo.GetIngredientByIdAsync(ingredientId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Ingredient)null);

            // Act & Assert
            await Assert.ThrowsAsync<IngredientRetrievalException>(() => _ingredientService.DeleteIngredientByIdAsync(ingredientId));
        }

        [Fact]
        public async Task SearchIngredientsAsync_ShouldReturnPaginatedResult()
        {
            // Arrange
            var ingredients = new List<Ingredient>
            {
                new Ingredient { Id = Guid.NewGuid(), Name = "Tomato", Description = "Fresh tomato", CoverImageUrl = new MediaUrl { Url = "http://example.com/tomato.jpg" } },
                new Ingredient { Id = Guid.NewGuid(), Name = "Potato", Description = "Fresh potato", CoverImageUrl = new MediaUrl { Url = "http://example.com/potato.jpg" } }
            };

            _ingredientRepositoryMock.Setup(repo => repo.GetIngredientsWithDetailsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(ingredients);

            // Act
            var result = await _ingredientService.SearchIngredientsAsync("Tom", 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal("Tomato", result.Items.First().IngredientName);
        }
    }
}
