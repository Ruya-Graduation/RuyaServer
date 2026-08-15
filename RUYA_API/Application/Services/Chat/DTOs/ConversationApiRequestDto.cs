using System.Text.Json.Serialization;

namespace RUYA_API.Application.Services.Chat.DTOs
{
    /// <summary>
    /// Request payload for POST /conversation endpoint in Python AI service
    /// </summary>
    public class ConversationApiRequestDto
    {
        [JsonPropertyName("artifact")]
        public ArtifactContextDto Artifact { get; set; } = new();
        
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;
        
        [JsonPropertyName("messages")]
        public List<MessageContextDto> Messages { get; set; } = new();
    }

    public class ArtifactContextDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;
        
        [JsonPropertyName("material")]
        public string Material { get; set; } = string.Empty;
        
        [JsonPropertyName("place_of_discovery")]
        public string PlaceOfDiscovery { get; set; } = string.Empty;
    }

    public class MessageContextDto
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
