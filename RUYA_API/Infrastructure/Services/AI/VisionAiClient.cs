using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
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

                // Prepare multipart form data
                using var content = new MultipartFormDataContent();
                using var fileStream = imageFile.OpenReadStream();
                using var streamContent = new StreamContent(fileStream);
                
                var contentType = string.IsNullOrWhiteSpace(imageFile.ContentType) ? "image/jpeg" : imageFile.ContentType;
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                content.Add(streamContent, "file", imageFile.FileName);

                // Add prediction parameters (conf, iou, imgsz)
                var confVal = (_settings.Conf > 0 ? _settings.Conf : 0.25).ToString(CultureInfo.InvariantCulture);
                var iouVal = (_settings.Iou > 0 ? _settings.Iou : 0.7).ToString(CultureInfo.InvariantCulture);
                var imgszVal = (_settings.Imgsz > 0 ? _settings.Imgsz : 640).ToString(CultureInfo.InvariantCulture);

                content.Add(new StringContent(confVal), "conf");
                content.Add(new StringContent(iouVal), "iou");
                content.Add(new StringContent(imgszVal), "imgsz");

                var endpoint = string.IsNullOrWhiteSpace(_settings.VisionEndpoint) ? "/predict" : _settings.VisionEndpoint;
                _logger.LogInformation("🔗 Calling vision endpoint: {Endpoint} with conf={Conf}, iou={Iou}, imgsz={Imgsz}", 
                    endpoint, confVal, iouVal, imgszVal);

                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                {
                    Content = content
                };

                // Add Authorization Bearer header if API key is provided
                if (!string.IsNullOrWhiteSpace(_settings.ApiKey))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _settings.ApiKey.Trim());
                }

                var response = await _httpClient.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogWarning("⚠️ Vision AI endpoint returned status {StatusCode}: {ErrorContent}", 
                        response.StatusCode, errorContent);

                    return new VisionResultDto
                    {
                        ArtifactId = null,
                        Confidence = 0.0,
                        ClassId = null,
                        DetectionsCount = 0,
                        Message = "Could not recognize the artifact in the provided image. Please provide a clearer photo."
                    };
                }

                var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogInformation("📦 Raw vision API response: {Response}", rawResponse);

                return ParseVisionResponse(rawResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error occurred while calling Vision AI endpoint.");
                return new VisionResultDto
                {
                    ArtifactId = null,
                    Confidence = 0.0,
                    ClassId = null,
                    DetectionsCount = 0,
                    Message = "Failed to process image recognition service. Please try again or type the artifact name."
                };
            }
        }

        private VisionResultDto ParseVisionResponse(string rawResponse)
        {
            try
            {
                using var doc = JsonDocument.Parse(rawResponse);
                var root = doc.RootElement;

                // Handle Ultralytics YOLO response format: { "images": [ { "results": [ ... ] } ] }
                if (root.TryGetProperty("images", out var imagesElement) && imagesElement.ValueKind == JsonValueKind.Array)
                {
                    var allDetections = new List<(string Name, int? ClassId, double Confidence)>();

                    foreach (var imageElement in imagesElement.EnumerateArray())
                    {
                        if (imageElement.TryGetProperty("results", out var resultsElement) && resultsElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var res in resultsElement.EnumerateArray())
                            {
                                string? name = null;
                                if (res.TryGetProperty("name", out var nProp)) name = nProp.GetString();
                                else if (res.TryGetProperty("class_name", out var cnProp)) name = cnProp.GetString();
                                else if (res.TryGetProperty("label", out var lProp)) name = lProp.GetString();

                                int? classId = null;
                                if (res.TryGetProperty("class", out var cProp) && cProp.TryGetInt32(out var cVal)) classId = cVal;
                                else if (res.TryGetProperty("class_id", out var ciProp) && ciProp.TryGetInt32(out var ciVal)) classId = ciVal;

                                double confidence = 0.0;
                                if (res.TryGetProperty("confidence", out var confProp) && confProp.TryGetDouble(out var confVal)) confidence = confVal;
                                else if (res.TryGetProperty("conf", out var cfProp) && cfProp.TryGetDouble(out var cfVal)) confidence = cfVal;

                                if (!string.IsNullOrWhiteSpace(name))
                                {
                                    allDetections.Add((name, classId, confidence));
                                }
                            }
                        }
                    }

                    if (allDetections.Count > 0)
                    {
                        var topDetection = allDetections.OrderByDescending(d => d.Confidence).First();
                        _logger.LogInformation("✅ Vision detected {Count} artifact(s). Top detection: '{Name}' (Confidence: {Confidence:P}, ClassId: {ClassId})", 
                            allDetections.Count, topDetection.Name, topDetection.Confidence, topDetection.ClassId);

                        return new VisionResultDto
                        {
                            ArtifactId = topDetection.Name,
                            Confidence = topDetection.Confidence,
                            ClassId = topDetection.ClassId,
                            DetectionsCount = allDetections.Count,
                            Message = null
                        };
                    }
                    else
                    {
                        _logger.LogInformation("ℹ️ Vision API returned 0 detections in 'results'.");
                        return new VisionResultDto
                        {
                            ArtifactId = null,
                            Confidence = 0.0,
                            ClassId = null,
                            DetectionsCount = 0,
                            Message = "No artifact detected in image. Try a clearer image or different angle."
                        };
                    }
                }

                // Fallback for direct DTO format: { "artifact_id": ..., "confidence": ... }
                if (root.TryGetProperty("artifact_id", out _) || root.TryGetProperty("artifactId", out _))
                {
                    var directResult = JsonSerializer.Deserialize<VisionResultDto>(rawResponse, new JsonSerializerOptions 
                    { 
                        PropertyNameCaseInsensitive = true 
                    });

                    if (directResult != null) return directResult;
                }

                _logger.LogWarning("⚠️ Unexpected JSON structure from Vision AI response.");
                return new VisionResultDto
                {
                    ArtifactId = null,
                    Confidence = 0.0,
                    ClassId = null,
                    DetectionsCount = 0,
                    Message = "No artifact detected in image. Try a clearer image or different angle."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to parse vision API JSON response.");
                return new VisionResultDto
                {
                    ArtifactId = null,
                    Confidence = 0.0,
                    ClassId = null,
                    DetectionsCount = 0,
                    Message = "Failed to parse detection result."
                };
            }
        }
    }
}
