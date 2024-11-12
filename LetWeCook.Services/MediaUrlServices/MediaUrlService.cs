using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.MediaUrlServices
{
    public class MediaUrlService : IMediaUrlService
    {
        private readonly IMediaUrlRepository _mediaUrlRepository;
        private readonly IUnitOfWork _unitOfWork;
        public MediaUrlService(IMediaUrlRepository mediaUrlRepository, IUnitOfWork unitOfWork)
        {
            _mediaUrlRepository = mediaUrlRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<MediaUrlDTO>> CreateMediaUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            MediaUrl mediaUrl = new MediaUrl
            {
                Id = Guid.NewGuid(),
                Url = url,
                Alt = url
            };

            Result<MediaUrl> result = await _mediaUrlRepository.CreateMediaUrlAsync(mediaUrl, cancellationToken);

            if (!result.IsSuccess)
            {
                return Result<MediaUrlDTO>.Failure("Failed to create media url", ErrorCode.MediaUrlCreationFailed, result.Exception);
            }

            int saveChangesResult;

            try
            {
                saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken); // Save changes

                if (saveChangesResult <= 0)
                {
                    return Result<MediaUrlDTO>.Failure("No changes were made to the database.", ErrorCode.MediaUrlCreationFailed);
                }
            }
            catch (Exception saveEx)
            {
                return Result<MediaUrlDTO>.Failure("Failed to save media url changes.", ErrorCode.MediaUrlCreationFailed, saveEx);
            }

            return Result<MediaUrlDTO>.Success(
                new MediaUrlDTO
                {
                    Id = result.Data!.Id,
                    Url = result.Data.Url,
                    Alt = result.Data.Alt
                }, "Create media url successfully");
        }
    }
}
