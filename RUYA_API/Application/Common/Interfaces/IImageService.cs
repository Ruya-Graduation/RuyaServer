namespace RUYA_API.Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<(string ImageUrl, string PublicId)> UploadImageAsync(IFormFile image);

        Task DeleteImageAsync(string publicId);
    }
}
