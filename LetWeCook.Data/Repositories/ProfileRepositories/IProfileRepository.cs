using LetWeCook.Data.Entities;

namespace LetWeCook.Data.Repositories.ProfileRepositories
{
    public interface IProfileRepository
    {
        /// <summary>
        /// Asynchronously retrieves a user profile by the associated user ID.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose profile is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests during the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a <see cref="UserProfile"/> entity, or <c>null</c> if no profile is found for the given user ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="userId"/> is <c>null</c> or invalid.</exception>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<UserProfile?> GetUserProfileByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously creates a new user profile and adds it to the database.
        /// </summary>
        /// <param name="profile">The user profile to be created.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the created <see cref="UserProfile"/> entity.</returns>
        /// <exception cref="OperationCanceledException">Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.</exception>
        Task<UserProfile> CreateUserProfile(UserProfile profile, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously updates the user profile in the database.
        /// </summary>
        /// <param name="profile">The <see cref="UserProfile"/> entity to be updated.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="profile"/> is <c>null</c>.</exception>
        Task UpdateUserProfile(UserProfile profile);
    }
}
