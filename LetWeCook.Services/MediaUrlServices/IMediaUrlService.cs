using LetWeCook.Services.DTOs;

namespace LetWeCook.Services.MediaUrlServices
{
    public interface IMediaUrlService
    {
        /// <summary>
        /// Creates a new media URL record without saving changes to the database.
        /// </summary>
        /// <param name="url">The URL of the media to be created.</param>
        /// <param name="cancellationToken">
        /// A token to observe for cancellation requests while performing the operation.
        /// </param>
        /// <returns>
        /// A <see cref="MediaUrlDTO"/> containing the details of the newly created media URL.
        /// </returns>
        /// <remarks>
        /// This method generates a new <see cref="Guid"/> for the media URL's ID and 
        /// sets the URL as both the <c>Url</c> and <c>Alt</c> fields.
        /// The created media URL is added to the repository but is not saved to the database.
        /// </remarks>
        Task<MediaUrlDTO> CreateMediaUrlAsync(string url, CancellationToken cancellationToken);
    }
}
