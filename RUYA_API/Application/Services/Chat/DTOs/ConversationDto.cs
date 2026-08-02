namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class ConversationDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string Status { get; set; } = string.Empty;

        public string CurrentLanguage { get; set; } = string.Empty;

        public string CurrentMode { get; set; } = string.Empty;

        public DateTime LastMessageAt { get; set; }
    }
}
