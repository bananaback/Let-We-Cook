using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.MediaUrlServices
{
    public interface IMediaUrlService
    {
        Task<Result<MediaUrlDTO>> CreateMediaUrlAsync(string url, CancellationToken cancellationToken);
    }
}
