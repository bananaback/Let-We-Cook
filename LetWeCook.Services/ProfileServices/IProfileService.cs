using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.ProfileServices
{
    public interface IProfileService
    {
        public Task<ProfileDTO> GetUserProfileAsync(string userId, CancellationToken cancellationToken);
        public Task<ProfileDTO> UpdateUserProfileAsync(ProfileDTO profileDTO, CancellationToken cancellationToken);
    }
}
