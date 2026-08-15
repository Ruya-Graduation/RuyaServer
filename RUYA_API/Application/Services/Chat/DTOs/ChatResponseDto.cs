namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class ChatResponseDto
    {
        public int ConversationId { get; set; }

        public string AssistantMessage { get; set; } = string.Empty;

        public int? CurrentArtifactId { get; set; }

        public bool UsedVision { get; set; }

        public bool NeedsNewFrame { get; set; }
    }
}
