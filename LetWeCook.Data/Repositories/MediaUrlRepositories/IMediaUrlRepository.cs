using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.MediaUrlRepositories
{
    public interface IMediaUrlRepository
    {
        Task<Result> DeleteMediaUrlAsync(Guid mediaUrlId, CancellationToken cancellationToken);

        Task<Result<MediaUrl>> CreateMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken);
        Task<Result<MediaUrl?>> GetMediaUrlById(Guid id, CancellationToken cancellationToken);
    }
}
