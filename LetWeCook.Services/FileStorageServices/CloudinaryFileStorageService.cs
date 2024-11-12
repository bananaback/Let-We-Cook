using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LetWeCook.Common.Enums;
using LetWeCook.Common.Results;
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

        // Method to create a simple upload preset for image and video uploads (ingredients/recipes)
        public UploadPresetResult CreateUploadPreset()
        {
            try
            {
                // Define the upload preset parameters
                var uploadPresetParams = new UploadPresetParams()
                {
                    Name = "my_preset", // Specify preset name
                    Unsigned = true, // Allow unsigned uploads
                    AllowedFormats = new string[] { "jpg", "png", "mp4", "mov" }, // Allow certain file types
                };

                // Call Cloudinary API to create the preset
                var result = _cloudinary.CreateUploadPreset(uploadPresetParams);

                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Return the result containing the upload preset info
                    return result;
                }
                else
                {
                    throw new Exception("Error creating upload preset: " + result.Error?.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception while creating upload preset: " + ex.Message);
            }
        }

        public async Task<Result<MediaUrl>> UploadFileAsync(string base64Image, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate and extract the image bytes
                if (string.IsNullOrWhiteSpace(base64Image))
                {
                    return Result<MediaUrl>.Failure("Base64 image string is null or empty.", ErrorCode.InvalidBase64Image);
                }

                byte[] imageBytes = Convert.FromBase64String(base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image);

                using (var imageStream = new MemoryStream(imageBytes))
                {
                    string fileName = $"ingredient_{Guid.NewGuid()}.png"; // Generate a unique filename

                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, imageStream)
                    };

                    // Step 1: Upload to Cloudinary
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return Result<MediaUrl>.Failure("Failed to upload image to Cloudinary.", ErrorCode.CloudinaryImageUploadFailed);
                    }

                    // Step 2: Create MediaUrl object
                    MediaUrl mediaUrl = new MediaUrl
                    {
                        Id = Guid.NewGuid(),
                        Url = uploadResult.SecureUrl.ToString(),
                        Alt = fileName // Using filename as alt text
                    };

                    // Step 3: Save MediaUrl in the repository (without committing changes yet)
                    var creationResult = await _mediaUrlRepository.CreateMediaUrlAsync(mediaUrl, cancellationToken);

                    if (!creationResult.IsSuccess)
                    {
                        // If saving fails, attempt to delete the image from Cloudinary
                        await DeleteImageFromCloudinary(uploadResult.PublicId);
                        return Result<MediaUrl>.Failure("Failed to save MediaUrl to the database. Image deleted from Cloudinary.", ErrorCode.MediaUrlCreationFailed, creationResult.Exception);
                    }

                    // Step 4: Persist changes using Unit of Work
                    var saveChangesResult = await _unitOfWork.SaveChangesAsync(cancellationToken);
                    if (saveChangesResult <= 0) // Check if no changes were saved
                    {
                        // If saving changes fails, attempt to delete the image from Cloudinary
                        await DeleteImageFromCloudinary(uploadResult.PublicId);
                        return Result<MediaUrl>.Failure("Failed to save changes to the database after MediaUrl creation. Image deleted from Cloudinary.", ErrorCode.MediaUrlSaveFailed);
                    }

                    return Result<MediaUrl>.Success(mediaUrl, "File uploaded and MediaUrl created and saved successfully.");
                }
            }
            catch (FormatException ex)
            {
                return Result<MediaUrl>.Failure("Invalid Base64 format.", ErrorCode.InvalidBase64Image, ex);
            }
            catch (Exception ex)
            {
                return Result<MediaUrl>.Failure("An unexpected error occurred during file upload.", ErrorCode.UnexpectedError, ex);
            }
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
                var deleteResult = await _mediaUrlRepository.DeleteMediaUrlAsync(mediaUrl.Id, cancellationToken);

                if (!deleteResult.IsSuccess)
                {
                    // Log the failure to delete MediaUrl from the database
                    _logger.LogWarning("Failed to delete MediaUrl from the database. MediaUrl ID: {MediaUrlId}. Error: {ErrorMessage}",
                        mediaUrl.Id, deleteResult.Exception?.Message);
                }
                else
                {
                    // Optionally, persist changes if necessary (though your repo should handle this if needed)
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
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
