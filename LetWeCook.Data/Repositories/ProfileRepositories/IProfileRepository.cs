using LetWeCook.Common.Results;
using LetWeCook.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetWeCook.Data.Repositories.ProfileRepositories
{
    public interface IProfileRepository
    {
        Task<Result<UserProfile>> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<Result<UserProfile>> CreateUserProfile(UserProfile profile, CancellationToken cancellationToken);
    }
}
