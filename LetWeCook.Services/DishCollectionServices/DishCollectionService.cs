using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.CollectionRecipeRepositories;
using LetWeCook.Data.Repositories.DishCollectionRepositories;
using LetWeCook.Data.Repositories.RecipeRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace LetWeCook.Services.DishCollectionServices
{
    public class DishCollectionService : IDishCollectionService
    {
        private readonly IDishCollectionRepository _dishCollectionRepository;
        private readonly ICollectionRecipeRepository _collectionRecipeRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public DishCollectionService(
            IDishCollectionRepository dishCollectionRepository,
            ICollectionRecipeRepository collectionRecipeRepository,
            IRecipeRepository recipeRepository,
            UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _dishCollectionRepository = dishCollectionRepository;
            _collectionRecipeRepository = collectionRecipeRepository;
            _recipeRepository = recipeRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<CollectionRecipeDTO> AddRecipeToCollectionAsync(string userId, Guid collectionId, Guid recipeId, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"User with id {userId} not found.");
            }

            var recipe = await _recipeRepository.GetRecipeDetailsByIdAsync(recipeId, cancellationToken);

            if (recipe == null)
            {
                throw new RecipeRetrievalException($"Recipe with id {recipeId} not found.");
            }

            var collection = await _dishCollectionRepository.GetDishCollectionByIdAsync(collectionId, cancellationToken);

            if (collection == null)
            {
                throw new DishCollectionRetrievalException($"Collection with id {collectionId} not found.");
            }

            CollectionRecipe collectionRecipe = new CollectionRecipe
            {
                Collection = collection,
                Recipe = recipe,
                DateAdded = DateTime.Now,
            };

            await _collectionRecipeRepository.CreateCollectionRecipeAsync(collectionRecipe, cancellationToken);

            var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (saveChangesResult <= 0) // Check if no changes were saved
            {
                throw new CollectionRecipeCreationException("Failed to save changes to the database after collection recipe creation.");
            }

            return new CollectionRecipeDTO
            {
                Collection = collection,
                Recipe = recipe,
                DateAdded = collectionRecipe.DateAdded
            };
        }

        public async Task<DishCollectionDTO> CreateDishCollectionAsync(string userId, DishCollectionDTO collectionDTO, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException($"User with id {userId} not found.");
            }

            DishCollection dishCollection = new DishCollection
            {
                Id = collectionDTO.Id,
                Name = collectionDTO.Name,
                Description = collectionDTO.Description,
                DateCreated = collectionDTO.DateCreated,
                User = user
            };

            await _dishCollectionRepository.CreateDishCollectionAsync(dishCollection, cancellationToken);

            var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (saveChangesResult <= 0) // Check if no changes were saved
            {
                throw new DishCollectionCreationException("Failed to save changes to the database after dish collection creation.");
            }

            return new DishCollectionDTO
            {
                Id = collectionDTO.Id,
                Name = collectionDTO.Name,
                Description = collectionDTO.Description,
                DateCreated = collectionDTO.DateCreated,
            };
        }

        public async Task<bool> DeleteDishCollectionAsync(string userId, Guid collectionId, CancellationToken cancellationToken)
        {
            // Check if the collection exists and belongs to the user
            var collection = await _dishCollectionRepository.GetDishCollectionByIdAsync(collectionId, cancellationToken);

            if (collection == null || collection.User.Id.ToString() != userId)
            {
                return false; // Return false if the collection is not found or doesn't belong to the user
            }

            // Proceed with the deletion
            await _dishCollectionRepository.DeleteDishCollectionAsync(collectionId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true; // Return true if deletion is successful
        }

        public async Task<DishCollectionDTO> GetCollectionDetailsByIdAsync(Guid collectionId, CancellationToken cancellationToken)
        {
            var collection = await _dishCollectionRepository.GetDishCollectionDetailsByIdAsync(collectionId, cancellationToken);

            if (collection == null)
            {
                throw new DishCollectionRetrievalException($"Dish collection with id {collectionId} not found.");
            }


            return new DishCollectionDTO
            {
                Id = collection!.Id,
                Name = collection.Name,
                Description = collection.Description,
                DateCreated = collection.DateCreated,
                Recipes = collection.Recipes.Select(r => new RecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    RecipeCoverImage = new MediaUrlDTO
                    {
                        Id = r.RecipeCoverImage!.Id,
                        Url = r.RecipeCoverImage.Url,
                        Alt = r.RecipeCoverImage.Alt
                    }
                }).ToList()
            };
        }

        public async Task<List<DishCollectionDTO>> GetUserDishCollectionsAsync(string userId, CancellationToken cancellationToken)
        {
            Guid userIdGuid;
            bool isValid = Guid.TryParse(userId, out userIdGuid);

            if (!isValid)
            {
                throw new UserNotFoundException($"Invalid user id {userId} detected while retrieving collections.");
            }

            var dishCollections = await _dishCollectionRepository.GetAllDishCollectionsByUserIdAsync(userIdGuid, cancellationToken);

            return dishCollections.Select(dc => new DishCollectionDTO
            {
                Id = dc.Id,
                Name = dc.Name,
                Description = dc.Description,
                DateCreated = dc.DateCreated,
                Recipes = dc.Recipes.Select(r => new RecipeDTO
                {
                    Id = r.Id,
                    Title = r.Title,
                    RecipeCoverImage = new MediaUrlDTO
                    {
                        Id = r.RecipeCoverImage!.Id,
                        Url = r.RecipeCoverImage.Url,
                        Alt = r.RecipeCoverImage.Alt
                    }
                }).ToList()

            }).ToList();
        }

        public async Task<bool> RemoveRecipeFromCollection(string userId, Guid collectionId, Guid recipeId, CancellationToken cancellationToken)
        {
            // Check if the collection exists and belongs to the user
            var collection = await _dishCollectionRepository.GetDishCollectionByIdAsync(collectionId, cancellationToken);

            if (collection == null || collection.User.Id.ToString() != userId)
            {
                return false; // Return false if the collection is not found or doesn't belong to the user
            }

            // Proceed with the deletion
            await _collectionRecipeRepository.DeleteCollectionRecipeAsync(collection.Id, recipeId, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true; // Return true if deletion is successful
        }
    }
}
