using Microsoft.AspNetCore.Http;
using RUYA_API.Application.Services.Chat.DTOs;

namespace RUYA_API.Application.Services.Chat.Interfaces
{
    public interface IVisionAiClient
    {
        Task<VisionResultDto?> RecognizeArtifactAsync(IFormFile imageFile, CancellationToken cancellationToken = default);
    }
}
