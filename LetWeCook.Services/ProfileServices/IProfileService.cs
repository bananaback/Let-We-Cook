using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.ProfileServices
{
    public interface IProfileService
    {
        public Task<Result<ProfileDTO>> GetUserProfileAsync(string userId, CancellationToken cancellationToken);
        public Task<Result<ProfileDTO>> UpdateUserProfileAsync(ProfileDTO profileDTO, CancellationToken cancellationToken);
    }
}
