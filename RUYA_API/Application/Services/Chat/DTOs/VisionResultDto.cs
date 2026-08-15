using System.Text.Json.Serialization;

namespace RUYA_API.Application.Services.Chat.DTOs
{
    /// <summary>
    /// Response from /detect-artifact endpoint
    /// Maps Python snake_case to C# PascalCase
    /// </summary>
    public class VisionResultDto
    {
        [JsonPropertyName("artifact_id")]
        public string? ArtifactId { get; set; } // Artifact name from Python API
        
        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
        
        [JsonPropertyName("class_id")]
        public int? ClassId { get; set; }
        
        [JsonPropertyName("detections_count")]
        public int? DetectionsCount { get; set; }
        
        [JsonPropertyName("message")]
        public string? Message { get; set; } // For when no artifact detected
        
        // Computed properties
        [JsonIgnore]
        public bool IsSuccess => Confidence >= 0.5 && !string.IsNullOrEmpty(ArtifactId);
        
        [JsonIgnore]
        public string? ClarificationMessage => Message ?? 
            (IsSuccess ? null : "No artifact detected in image. Try a clearer image or different angle.");
    }
}
