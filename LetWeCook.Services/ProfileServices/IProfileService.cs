using LetWeCook.Common.Results;
using LetWeCook.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetWeCook.Services.ProfileServices
{
    public interface IProfileService
    {
        public Task<Result<ProfileDTO>> GetUserProfileAsync(string userId, CancellationToken cancellationToken);
    }
}
