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

        /// <summary>
        /// Asynchronously adds a new media URL to the database.
        /// </summary>
        /// <param name="mediaUrl">The media URL to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>The <see cref="MediaUrl"/> object that was added to the database.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<MediaUrl> AddMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken = default)
        {
            await _context.MediaUrls.AddAsync(mediaUrl, cancellationToken);
            return mediaUrl;
        }


        /// <summary>
        /// Asynchronously deletes a media URL by its unique identifier.
        /// </summary>
        /// <param name="mediaUrlId">The unique identifier of the media URL to be deleted.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation. It completes when the media URL is deleted.</returns>
        public async Task DeleteMediaUrlByIdAsync(Guid mediaUrlId, CancellationToken cancellationToken = default)
        {
            // Find the MediaUrl entity by its Id
            var mediaUrl = await _context.MediaUrls.FindAsync(mediaUrlId, cancellationToken);

            if (mediaUrl == null)
            {
                // Handle the case where the media URL does not exist (returning null or handling logic is done in the service layer)
                return;
            }

            // Remove the entity from the DbSet
            _context.MediaUrls.Remove(mediaUrl);

            // The changes will be saved in the service layer or unit of work
        }


        /// <summary>
        /// Asynchronously retrieves a media URL by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the media URL to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the <see cref="MediaUrl"/> entity, or <c>null</c> if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="id"/> is <c>null</c> or invalid.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<MediaUrl?> GetMediaUrlByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            MediaUrl? mediaUrl = await _context.MediaUrls
                .Where(mu => mu.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return mediaUrl;
        }

        /// <summary>
        /// Asynchronously retrieves a list of media URLs by their unique identifiers.
        /// </summary>
        /// <param name="idList">A list of unique identifiers of the media URLs to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a list of <see cref="MediaUrl"/> entities.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="idList"/> is <c>null</c> or contains <c>null</c> identifiers.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        public async Task<List<MediaUrl>> GetMediaUrlsByIdsAsync(List<Guid> idList, CancellationToken cancellationToken = default)
        {
            List<MediaUrl> mediaUrls = await _context.MediaUrls
                .Where(m => idList.Contains(m.Id))
                .ToListAsync(cancellationToken);

            return mediaUrls;
        }

    }
}
