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
        public async Task<MediaUrlDTO> CreateMediaUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            MediaUrl mediaUrl = new MediaUrl
            {
                Id = Guid.NewGuid(),
                Url = url,
                Alt = url
            };

            await _mediaUrlRepository.AddMediaUrlAsync(mediaUrl, cancellationToken);

            return new MediaUrlDTO
            {
                Id = mediaUrl.Id,
                Url = mediaUrl.Url,
                Alt = mediaUrl.Alt,
            };
        }
    }
}
