namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class AiChatRequestDto
    {
        public int? ArtifactId { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<ChatMessageHistoryDto> History { get; set; } = new();
        public string Language { get; set; } = "en";
        public string Mode { get; set; } = "story";
    }

    public class ChatMessageHistoryDto
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
