using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LetWeCook.Data.Repositories.MediaUrlRepositories
{
    public class MediaUrlRepository : IMediaUrlRepository
    {
        private readonly LetWeCookDbContext _context;
        public MediaUrlRepository(LetWeCookDbContext context)
        {
            _context = context;
        }
        public async Task<Result<MediaUrl>> CreateMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.MediaUrls.AddAsync(mediaUrl, cancellationToken);

                return Result<MediaUrl>.Success(mediaUrl, "Media URL created successfully.");
            }
            catch (Exception ex)
            {
                return Result<MediaUrl>.Failure("An error occurred while creating the media URL.", ErrorCode.MediaUrlCreationFailed, ex);
            }

        }

        public async Task<Result> DeleteMediaUrlAsync(Guid mediaUrlId, CancellationToken cancellationToken = default)
        {
            try
            {
                // Find the MediaUrl entity by its Id
                var mediaUrl = await _context.MediaUrls.FindAsync(mediaUrlId, cancellationToken);

                if (mediaUrl == null)
                {
                    return Result.Failure("Media URL not found.", ErrorCode.MediaUrlNotFound);
                }

                // Remove the entity from the DbSet
                _context.MediaUrls.Remove(mediaUrl);

                // Return success message without saving changes here; handle saving in the Unit of Work
                return Result.Success("Media URL deleted successfully.");
            }
            catch (Exception ex)
            {
                return Result.Failure("An error occurred while deleting the media URL.", ErrorCode.MediaUrlDeletionFailed, ex);
            }
        }

        public async Task<Result<MediaUrl?>> GetMediaUrlById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                MediaUrl? mediaUrl = await _context.MediaUrls
                    .Where(mu => mu.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);
                return Result<MediaUrl?>.Success(mediaUrl, "Retrieve media url success (maybe null if not found).");
            }
            catch (Exception ex)
            {
                return Result<MediaUrl?>.Failure("Failed to retrieve media url", ErrorCode.MediaUrlRetrievalFailed, ex);
            }
        }

        public async Task<Result<List<MediaUrl>>> GetMediaUrlByIdList(List<Guid> idList, CancellationToken cancellationToken)
        {
            try
            {
                List<MediaUrl> mediaUrls = await _context.MediaUrls
                    .Where(m => idList.Contains(m.Id))
                    .ToListAsync(cancellationToken);

                return Result<List<MediaUrl>>.Success(mediaUrls, "Media urls retrieved successfully by IDs.");
            }
            catch (Exception ex)
            {
                return Result<List<MediaUrl>>.Failure("Failed to retrieve media urls by IDs", ErrorCode.MediaUrlRetrievalFailed, ex);
            }
        }
    }
}
