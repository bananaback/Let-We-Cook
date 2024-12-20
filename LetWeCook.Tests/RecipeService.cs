using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.RecipeReviewServices;
using LetWeCook.Services.RecipeServices;
using Microsoft.AspNetCore.Identity;
using Moq;

public class RecipeServiceTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock = new();
    private readonly Mock<IMediaUrlRepository> _mediaUrlRepositoryMock = new();
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IRecipeReviewService> _recipeReviewServiceMock = new();
    private readonly RecipeService _recipeService;

    public RecipeServiceTests()
    {
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null, null, null, null, null, null, null, null);

        _recipeService = new RecipeService(
            _recipeRepositoryMock.Object,
            _mediaUrlRepositoryMock.Object,
            _ingredientRepositoryMock.Object,
            _recipeReviewServiceMock.Object,
            _unitOfWorkMock.Object,
            _userManagerMock.Object);
    }


    [Fact]
    public async Task CreateRecipeAsync_MediaUrlNotFound_ThrowsException()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var recipeDTO = new RecipeDTO
        {
            RecipeCoverImage = new MediaUrlDTO { Id = Guid.NewGuid() }
        };

        _mediaUrlRepositoryMock.Setup(x => x.GetMediaUrlByIdAsync(recipeDTO.RecipeCoverImage.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MediaUrl)null);

        // Act & Assert
        await Assert.ThrowsAsync<RecipeCreationException>(() =>
            _recipeService.CreateRecipeAsync(userId, recipeDTO, CancellationToken.None));
    }

    [Fact]
    public async Task CreateRecipeAsync_UserNotFound_ThrowsException()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var recipeDTO = new RecipeDTO
        {
            RecipeCoverImage = new MediaUrlDTO { Id = Guid.NewGuid() }
        };

        _mediaUrlRepositoryMock.Setup(x => x.GetMediaUrlByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MediaUrl());
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync((ApplicationUser)null);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(() =>
            _recipeService.CreateRecipeAsync(userId, recipeDTO, CancellationToken.None));
    }

    [Fact]
    public async Task CreateRecipeAsync_IngredientNotFound_ThrowsException()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        var recipeDTO = new RecipeDTO
        {
            RecipeCoverImage = new MediaUrlDTO { Id = Guid.NewGuid() },
            RecipeIngredientDTOs = new List<RecipeIngredientDTO>
            {
                new RecipeIngredientDTO { IngredientId = Guid.NewGuid() }
            }
        };

        _mediaUrlRepositoryMock.Setup(x => x.GetMediaUrlByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MediaUrl());
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(new ApplicationUser());
        _ingredientRepositoryMock.Setup(x => x.GetIngredientsByIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Ingredient>()); // No ingredients found

        // Act & Assert
        await Assert.ThrowsAsync<RecipeCreationException>(() =>
            _recipeService.CreateRecipeAsync(userId, recipeDTO, CancellationToken.None));
    }
}
