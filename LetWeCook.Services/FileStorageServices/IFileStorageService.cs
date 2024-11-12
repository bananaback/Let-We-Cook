using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;

namespace LetWeCook.Services.FileStorageServices
{
    public interface IFileStorageService
    {
        Task<Result<MediaUrl>> UploadFileAsync(string base64Image, CancellationToken cancellationToken);
        Task DeleteMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken);
    }
}
