using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LetWeCook.Data.Entities;
using LetWeCook.Data.Repositories.MediaUrlRepositories;
using LetWeCook.Data.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace LetWeCook.Services.FileStorageServices
{
    public class CloudinaryFileStorageService : IFileStorageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly IMediaUrlRepository _mediaUrlRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CloudinaryFileStorageService> _logger;

        public CloudinaryFileStorageService(
            Cloudinary cloudinary,
            IMediaUrlRepository mediaUrlRepository,
            IUnitOfWork unitOfWork,
            ILogger<CloudinaryFileStorageService> logger)
        {
            _cloudinary = cloudinary;
            _mediaUrlRepository = mediaUrlRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task DeleteMediaUrlAsync(MediaUrl mediaUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                // Step 1: Extract the public ID from the MediaUrl URL
                string publicId = GetPublicIdFromUrl(mediaUrl.Url);

                // Step 2: Delete the image from Cloudinary
                var deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

                // Check if deletion was successful
                if (deletionResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    // Log the failure to delete the image from Cloudinary
                    _logger.LogWarning("Failed to delete image from Cloudinary. Public ID: {PublicId}. Error: {ErrorMessage}",
                        publicId, deletionResult.Error?.Message);
                }

                // Step 3: Delete the MediaUrl from the database
                await _mediaUrlRepository.DeleteMediaUrlByIdAsync(mediaUrl.Id, cancellationToken);
                // Gracefully save changes after delete media url
                await _unitOfWork.SaveChangesAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                // Log the unexpected exception during MediaUrl deletion
                _logger.LogError("An unexpected error occurred while deleting MediaUrl ID: {MediaUrlId}. Error: {ErrorMessage}",
                    mediaUrl.Id, ex.Message);
            }
        }


        // Helper method to delete image from Cloudinary
        private async Task DeleteImageFromCloudinary(string publicId)
        {
            var deletionResult = await _cloudinary.DestroyAsync(new DeletionParams(publicId));

            if (deletionResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // Log the failure to delete the image
                _logger.LogError("Failed to delete image from Cloudinary. Public ID: {PublicId}. Status Code: {StatusCode}.", publicId, deletionResult.StatusCode);
            }
        }

        // Helper method to extract the public ID from the file URL
        private string GetPublicIdFromUrl(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var segments = uri.Segments;

            // Assuming the public ID is the segment just before the last segment (which is the file extension)
            // Example URL: https://res.cloudinary.com/your-cloud-name/image/upload/v1628169624/sample.jpg
            // Here, sample is the public ID
            string publicId = segments[segments.Length - 1].Split('.')[0]; // Remove the extension
            return publicId;
        }
    }
}
