using LetWeCook.Common;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.IngredientRepositories;
using LetWeCook.Data.Repositories.IngredientSectionRepositories;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;
using LetWeCook.Services.Exceptions;
using LetWeCook.Services.FileStorageServices;
using LetWeCook.Web.Areas.Cooking.Models.DTOs;

namespace LetWeCook.Services.IngredientServices
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMediaUrlRepository _mediaUrlRepository;
        private readonly IIngredientSectionRepository _ingredientSectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const int MaxLevenshteinDistance = 3; // Adjust as needed for fuzzy tolerance
        public IngredientService(
            IIngredientRepository ingredientRepository,
            IMediaUrlRepository mediaUrlRepository,
            IIngredientSectionRepository ingredientSectionRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _ingredientRepository = ingredientRepository;
            _mediaUrlRepository = mediaUrlRepository;
            _ingredientSectionRepository = ingredientSectionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IngredientDTO> CreateIngredientAsync(RawIngredientDTO ingredientDTO, CancellationToken cancellationToken = default)
        {
            // Step 1: Persist cover image
            MediaUrl coverImageUrl = await PersistMediaUrlAsync(ingredientDTO.CoverImageUrl, cancellationToken);

            // Step 2: Prepare ingredient
            Ingredient ingredient = new Ingredient
            {
                Name = ingredientDTO.IngredientName,
                Description = ingredientDTO.IngredientDescription,
                CoverImageUrl = coverImageUrl
            };

            // Step 3: Persist ingredient sections
            ingredient.IngredientSections = await PersistIngredientSectionsAsync(ingredientDTO.RawFrameDTOs, ingredient, cancellationToken);

            // Step 4: Persist ingredient
            await _ingredientRepository.CreateIngredientAsync(ingredient, cancellationToken);

            // Step 5: Save changes in a whole with unit of work
            int saveChangesResult = await SaveChangesAsync(cancellationToken);

            // Return the resulting DTO
            return new IngredientDTO();
        }

        private async Task<MediaUrl> PersistMediaUrlAsync(string url, CancellationToken cancellationToken)
        {
            var mediaUrl = new MediaUrl
            {
                Id = Guid.NewGuid(),
                Url = url,
                Alt = url
            };

            await _mediaUrlRepository.AddMediaUrlAsync(mediaUrl, cancellationToken);
            return mediaUrl;
        }

        private async Task<List<IngredientSection>> PersistIngredientSectionsAsync(IEnumerable<RawFrameDTO> frames, Ingredient ingredient, CancellationToken cancellationToken)
        {
            var sections = new List<IngredientSection>();

            foreach (var frame in frames)
            {
                var section = new IngredientSection
                {
                    Id = Guid.NewGuid(),
                    Order = frame.Order,
                    Ingredient = ingredient
                };

                if (frame.ContentType == "image")
                {
                    section.MediaUrl = await PersistMediaUrlAsync(frame.ImageUrl, cancellationToken);
                }
                else if (frame.ContentType == "text")
                {
                    section.TextContent = frame.TextContent;
                }

                await _ingredientSectionRepository.AddIngredientSectionAsync(section, cancellationToken);
                sections.Add(section);
            }

            return sections;
        }

        private async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new IngredientCreationException("Failed to save changes while creating ingredient.", ex);
            }
        }


        public async Task<PaginatedResult<IngredientDTO>> SearchIngredientsAsync(string name = "", int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            if (pageSize <= 0)
            {
                throw new IngredientRetrievalException($"Invalid page size : {pageSize}");
            }

            try
            {
                var ingredients = await _ingredientRepository.GetIngredientsWithDetailsAsync(cancellationToken);
                // Apply fuzzy search using Levenshtein distance
                var filteredIngredients = string.IsNullOrEmpty(name)
                    ? ingredients
                    : ingredients!
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


                return new PaginatedResult<IngredientDTO>(pagedIngredients, page, pageSize, totalItems);
            }
            catch (ArgumentNullException ex)
            {
                throw new IngredientRetrievalException("Failed to search ingredients", ex);
            }


        }

        public async Task<IngredientDTO> GetIngredientByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                Ingredient? ingredient = await _ingredientRepository.GetIngredientWithDetailsByIdAsync(id, cancellationToken);
                if (ingredient == null)
                {
                    throw new IngredientRetrievalException($"Ingredient with id {id} not found.");
                }

                return new IngredientDTO
                {
                    Id = ingredient.Id,
                    IngredientName = ingredient.Name,
                    IngredientDescription = ingredient.Description,
                    CoverImageUrl = ingredient.CoverImageUrl?.Url ?? "",
                    Frames = ingredient.IngredientSections.Select(x => new FrameDTO
                    {
                        Id = x.Id,
                        MediaUrl = x.MediaUrl?.Url ?? string.Empty, // Use empty string if MediaUrl is null
                        TextContent = x.TextContent,
                        Order = x.Order
                    }).OrderBy(f => f.Order).ToList()
                };
            }
            catch (ArgumentNullException ex)
            {
                throw new IngredientRetrievalException($"Failed to retrieve ingredient with id {id}", ex);
            }

        }

        public async Task<List<IngredientDTO>> GetIngredientsNameAndIdAsync(CancellationToken cancellationToken = default)
        {
            List<Ingredient> result = await _ingredientRepository.GetAllIngredientIdsAndNamesAsync(cancellationToken);

            List<IngredientDTO> ingredientDTOs = result.Select(i => new IngredientDTO
            {
                Id = i.Id,
                IngredientName = i.Name,
            }).ToList();

            return ingredientDTOs;

        }
    }
}
