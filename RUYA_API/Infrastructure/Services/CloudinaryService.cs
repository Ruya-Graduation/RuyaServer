using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.ExceptionHandling.CustomException;

namespace RUYA_API.Infrastructure.Services
{
    public class CloudinaryService : IImageService
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        private static readonly string[] AllowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public CloudinaryService(IOptions<CloudinarySettings> options)
        {
            var settings = options.Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret);

            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task<(string ImageUrl, string PublicId)> UploadImageAsync(IFormFile image)
        {
            ValidateImage(image);

            try
            {
                await using var stream = image.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(image.FileName, stream),
                    Folder = "RUYA/Artifacts"
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.Error is not null)
                {
                    throw new AppException(
                        result.Error.Message,
                        StatusCodes.Status500InternalServerError);
                }

                return (result.SecureUrl.ToString(), result.PublicId);
            }
            catch (AppException)
            {
                throw;
            }
            catch
            {
                throw new AppException(
                    "Failed to upload image.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            try
            {
                var deleteParams = new DeletionParams(publicId);

                var result = await _cloudinary.DestroyAsync(deleteParams);

                if (result.Error is not null)
                {
                    throw new AppException(
                        result.Error.Message,
                        StatusCodes.Status500InternalServerError);
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch
            {
                throw new AppException(
                    "Failed to delete image.",
                    StatusCodes.Status500InternalServerError);
            }
        }

        private static void ValidateImage(IFormFile image)
        {
            if (image is null)
            {
                throw new AppException(
                    "Image is required.",
                    StatusCodes.Status400BadRequest);
            }

            if (image.Length == 0)
            {
                throw new AppException(
                    "Image file is empty.",
                    StatusCodes.Status400BadRequest);
            }

            if (image.Length > MaxFileSize)
            {
                throw new AppException(
                    "Image size must not exceed 5 MB.",
                    StatusCodes.Status400BadRequest);
            }

            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
            {
                throw new AppException(
                    "Only JPG, JPEG, PNG and WEBP images are allowed.",
                    StatusCodes.Status400BadRequest);
            }
        }
    }
}
