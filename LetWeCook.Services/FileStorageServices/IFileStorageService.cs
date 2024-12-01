using LetWeCook.Data.Entities;
namespace LetWeCook.Services.FileStorageServices
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Deletes the specified media file from the storage service and removes its metadata from the database.
        /// </summary>
        /// <param name="mediaUrl">The <see cref="MediaUrl"/> object representing the media to delete.</param>
        /// <param name="cancellationToken">
        /// A token to observe for cancellation requests. The operation will attempt to stop gracefully if canceled.
        /// </param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="number">
        /// <item>Extracts the public ID from the URL of the specified media.</item>
        /// <item>Attempts to delete the media file from the storage service (e.g., Cloudinary).</item>
        /// <item>Deletes the associated metadata from the database.</item>
        /// </list>
        /// Logs any errors encountered during the deletion process but does not throw exceptions for failures in media deletion.
        /// </remarks>
        /// <exception cref="OperationCanceledException">
        /// Thrown if the operation is canceled through the <paramref name="cancellationToken"/>.
        /// </exception>
        Task DeleteMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken);
    }
}
