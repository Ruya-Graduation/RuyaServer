using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RUYA_API.Application.Services.Chat.DTOs;
using RUYA_API.Application.Services.Chat.Interfaces;

namespace RUYA_API.Infrastructure.Services.AI
{
    public class VisionAiClient : IVisionAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AIServiceSettings _settings;
        private readonly ILogger<VisionAiClient> _logger;

        public VisionAiClient(HttpClient httpClient, IOptions<AIServiceSettings> settings, ILogger<VisionAiClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<VisionResultDto?> RecognizeArtifactAsync(IFormFile imageFile, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("📸 Starting vision recognition for file: {FileName} ({Size} bytes)", 
                    imageFile.FileName, imageFile.Length);

                // Prepare multipart form data with the image file
                using var content = new MultipartFormDataContent();
                using var fileStream = imageFile.OpenReadStream();
                using var streamContent = new StreamContent(fileStream);
                
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(streamContent, "file", imageFile.FileName);

                _logger.LogInformation("🔗 Calling vision endpoint: {Endpoint}", _settings.VisionEndpoint);
                
                var response = await _httpClient.PostAsync(_settings.VisionEndpoint, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("⚠️  Vision AI endpoint returned status {StatusCode}: {ErrorContent}", 
                        response.StatusCode, errorContent);
                    return new VisionResultDto
                    {
                        Confidence = 0.0,
                        Message = "Could not recognize the artifact in the provided image. Please provide a clearer photo."
                    };
                }

                // Read the raw response for debugging
                var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("📦 Raw vision API response: {Response}", rawResponse);

                // Deserialize
                var result = System.Text.Json.JsonSerializer.Deserialize<VisionResultDto>(
                    rawResponse, 
                    new System.Text.Json.JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                if (result != null)
                {
                    _logger.LogInformation("✅ Vision result: ArtifactId={ArtifactId}, Confidence={Confidence:P}, ClassId={ClassId}", 
                        result.ArtifactId, result.Confidence, result.ClassId);
                }
                else
                {
                    _logger.LogWarning("⚠️  Failed to deserialize vision response");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while calling Vision AI endpoint.");
                // Return default unconfident result on network/AI error
                return new VisionResultDto
                {
                    Confidence = 0.0,
                    Message = "Failed to process image recognition service. Please try again or type the artifact name."
                };
            }
        }
    }
}
