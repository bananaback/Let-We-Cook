using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.MediaUrlRepositories
{
    public interface IMediaUrlRepository
    {
        Task DeleteMediaUrlByIdAsync(Guid mediaUrlId, CancellationToken cancellationToken);

        Task<MediaUrl> AddMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken);
        Task<MediaUrl?> GetMediaUrlByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<MediaUrl>> GetMediaUrlsByIdsAsync(List<Guid> idList, CancellationToken cancellationToken);
    }
}
