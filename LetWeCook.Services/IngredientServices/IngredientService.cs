using LetWeCook.Common;
using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.FileStorageServices;
using LetWeCook.Web.Areas.Cooking.Models.DTOs;

namespace LetWeCook.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorageService;

        private const int MaxLevenshteinDistance = 3; // Adjust as needed for fuzzy tolerance
        public IngredientService(
            IIngredientRepository ingredientRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<IngredientDTO>> CreateIngredientAsync(RawIngredientDTO ingredientDTO, CancellationToken cancellationToken = default)
        {
            List<MediaUrl> mediaUrls = new List<MediaUrl>(); // To hold media URLs for cleanup later
            MediaUrl? uploadedCoverImageUrl = null; // Store cover image URL to delete if necessary

            try
            {
                // Step 1: Upload Cover Image
                Result<MediaUrl> uploadCoverImageResult = await _fileStorageService.UploadFileAsync(ingredientDTO.CoverImageBase64, cancellationToken);
                if (!uploadCoverImageResult.IsSuccess)
                {
                    // Log the error and return failure
                    return Result<IngredientDTO>.Failure("Create ingredient failed due to failed attempt uploading cover image", ErrorCode.IngredientCreationFailed);
                }

                uploadedCoverImageUrl = uploadCoverImageResult.Data; // Store the cover image URL

                // Step 2: Upload Frame Images
                foreach (var rawFrame in ingredientDTO.RawFrameDTOs)
                {
                    if (rawFrame.ContentType == "image")
                    {
                        Result<MediaUrl> result = await _fileStorageService.UploadFileAsync(rawFrame.ImageContent, cancellationToken);
                        if (!result.IsSuccess)
                        {
                            // Log the error and delete uploaded cover image
                            await DeleteUploadedImages(uploadedCoverImageUrl, mediaUrls, cancellationToken); // Clean up before returning
                            return Result<IngredientDTO>.Failure("Create ingredient failed due to failed attempt uploading frame image", ErrorCode.IngredientCreationFailed);
                        }
                        mediaUrls.Add(result.Data!);
                    }
                }

                // Step 3: Create Ingredient Sections
                List<IngredientSection> ingredientSections = CreateIngredientSections(ingredientDTO.RawFrameDTOs, mediaUrls);

                // Step 4: Create Ingredient
                Ingredient ingredient = new Ingredient
                {
                    Name = ingredientDTO.IngredientName,
                    Description = ingredientDTO.IngredientDescription,
                    CoverImageUrl = uploadedCoverImageUrl!, // Assuming MediaUrl has a property Url
                    IngredientSections = ingredientSections
                };

                // Attempt to create the ingredient in the repository
                Result<Ingredient> createIngredientResult = await _ingredientRepository.CreateIngredientAsync(ingredient, cancellationToken);
                if (!createIngredientResult.IsSuccess)
                {
                    // Call delete method for images before returning failure
                    await DeleteUploadedImages(uploadedCoverImageUrl, mediaUrls, cancellationToken);
                    return Result<IngredientDTO>.Failure("Create ingredient failed", ErrorCode.IngredientCreationFailed, createIngredientResult.Exception);
                }

                // Step 5: Save changes with unit of work
                int saveChangesResult;
                try
                {
                    saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken); // Save changes

                    if (saveChangesResult <= 0)
                    {
                        // No changes were saved, handle this case
                        await DeleteUploadedImages(uploadedCoverImageUrl, mediaUrls, cancellationToken);
                        return Result<IngredientDTO>.Failure("No changes were made to the database.", ErrorCode.IngredientCreationFailed);
                    }
                }
                catch (Exception saveEx)
                {
                    // Call delete method for images before returning failure
                    await DeleteUploadedImages(uploadedCoverImageUrl, mediaUrls, cancellationToken);
                    return Result<IngredientDTO>.Failure("Failed to save ingredient changes.", ErrorCode.IngredientCreationFailed, saveEx);
                }

                return Result<IngredientDTO>.Success(new IngredientDTO(), "Create ingredient successfully");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                // Call delete method for images before returning failure
                await DeleteUploadedImages(uploadedCoverImageUrl, mediaUrls, cancellationToken);
                return Result<IngredientDTO>.Failure("An error occurred during ingredient creation.", ErrorCode.IngredientCreationFailed, ex);
            }
        }

        // Method to delete uploaded images from file storage
        private async Task DeleteUploadedImages(MediaUrl? coverImageUrl, List<MediaUrl> mediaUrls, CancellationToken cancellationToken)
        {
            if (coverImageUrl != null)
            {
                await _fileStorageService.DeleteMediaUrlAsync(coverImageUrl, cancellationToken); // Delete cover image
            }

            foreach (var mediaUrl in mediaUrls)
            {
                await _fileStorageService.DeleteMediaUrlAsync(mediaUrl, cancellationToken); // Delete each frame image
            }
        }

        // Helper method to create ingredient sections
        private List<IngredientSection> CreateIngredientSections(List<RawFrameDTO> rawFrameDTOs, List<MediaUrl> mediaUrls)
        {
            List<IngredientSection> ingredientSections = new List<IngredientSection>();
            int imageIndex = 0;

            foreach (var rawFrame in rawFrameDTOs)
            {
                IngredientSection ingredientSection = new IngredientSection
                {
                    Id = Guid.NewGuid(),
                    MediaUrl = rawFrame.ContentType == "image" ? mediaUrls.ElementAtOrDefault(imageIndex) : null,
                    TextContent = rawFrame.ContentType == "text" ? rawFrame.TextContent : string.Empty,
                    Order = rawFrame.Order
                };

                ingredientSections.Add(ingredientSection);

                if (rawFrame.ContentType == "image")
                {
                    imageIndex++;
                }
            }

            return ingredientSections;
        }

        public async Task<Result<PaginatedResult<IngredientDTO>>> SearchIngredientsAsync(string name = "", int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (pageSize != 5 && pageSize != 10 && pageSize != 20 && pageSize != 30 && pageSize != 50)
            {
                pageSize = 10; // Default page size
            }

            var result = await _ingredientRepository.GetAllIngredientsWithCoverImageAndSectionsAsync(cancellationToken);
            if (!result.IsSuccess)
            {
                return Result<PaginatedResult<IngredientDTO>>.Failure(
                    "Failed to search ingredients",
                    ErrorCode.IngredientRetrievalFailed,
                    result.Exception);
            }

            List<Ingredient> allIngredients = result.Data!;

            // Apply fuzzy search using Levenshtein distance
            var filteredIngredients = string.IsNullOrEmpty(name)
                ? allIngredients
                : allIngredients!
                    .Where(i => i.Name.LevenshteinDistance(name) <= MaxLevenshteinDistance)
                    .ToList();

            int totalItems = filteredIngredients.Count;
            var pagedIngredients = filteredIngredients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new IngredientDTO
                {
                    Id = i.Id,
                    IngredientName = i.Name,
                    IngredientDescription = i.Description,
                    CoverImageUrl = i.CoverImageUrl.Url,
                    Frames = i.IngredientSections.Select(x => new FrameDTO
                    {
                        Id = x.Id,
                        MediaUrl = x.MediaUrl?.Url ?? string.Empty, // Use empty string if MediaUrl is null
                        TextContent = x.TextContent,
                        Order = x.Order
                    }).OrderBy(f => f.Order).ToList()
                })
                .ToList();


            var paginatedResult = new PaginatedResult<IngredientDTO>(pagedIngredients, page, pageSize, totalItems);
            return Result<PaginatedResult<IngredientDTO>>.Success(paginatedResult, "Retrieve ingredients successfully.");
        }

        public async Task<Result<IngredientDTO>> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            Result<Ingredient?> result = await _ingredientRepository.GetInredientWithCoverImageAndSectionByIdAsync(id, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<IngredientDTO>.Failure($"Retrieve ingredient with id: {id} failed.", ErrorCode.IngredientRetrievalFailed, result.Exception);
            }

            if (result.Data == null)
            {
                return Result<IngredientDTO>.Failure("Ingredient with id: {id} not found.", ErrorCode.IngredientNotFound, null);
            }

            Ingredient i = result.Data!;

            return Result<IngredientDTO>.Success(new IngredientDTO
            {
                Id = i.Id,
                IngredientName = i.Name,
                IngredientDescription = i.Description,
                CoverImageUrl = i.CoverImageUrl?.Url ?? "",
                Frames = i.IngredientSections.Select(x => new FrameDTO
                {
                    Id = x.Id,
                    MediaUrl = x.MediaUrl?.Url ?? string.Empty, // Use empty string if MediaUrl is null
                    TextContent = x.TextContent,
                    Order = x.Order
                }).OrderBy(f => f.Order).ToList()
            });
        }

        public async Task<Result<List<IngredientDTO>>> GetIngredientsNameAndIdAsync(CancellationToken cancellationToken = default)
        {
            Result<List<Ingredient>> result = await _ingredientRepository.GetAllIngredientsNameAndIdAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<List<IngredientDTO>>.Failure(
                    "Failed to get all ingredients name",
                    ErrorCode.IngredientRetrievalFailed,
                    result.Exception);
            }

            List<IngredientDTO> ingredientDTOs = result.Data!.Select(i => new IngredientDTO
            {
                Id = i.Id,
                IngredientName = i.Name,
            }).ToList();

            return Result<List<IngredientDTO>>.Success(ingredientDTOs, "Retrieve ingredient names successfully.");

        }
    }
}
