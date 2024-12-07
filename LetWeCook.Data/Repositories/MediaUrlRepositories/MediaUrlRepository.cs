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


        public async Task<MediaUrl> AddMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken = default)
        {
            await _context.MediaUrls.AddAsync(mediaUrl, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return mediaUrl;
        }



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



        public async Task<MediaUrl?> GetMediaUrlByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            MediaUrl? mediaUrl = await _context.MediaUrls
                .Where(mu => mu.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            return mediaUrl;
        }


        public async Task<List<MediaUrl>> GetMediaUrlsByIdsAsync(List<Guid> idList, CancellationToken cancellationToken = default)
        {
            List<MediaUrl> mediaUrls = await _context.MediaUrls
                .Where(m => idList.Contains(m.Id))
                .ToListAsync(cancellationToken);

            return mediaUrls;
        }

    }
}
