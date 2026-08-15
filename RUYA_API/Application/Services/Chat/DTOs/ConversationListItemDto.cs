namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class ConversationListItemDto
    {
        public int ConversationId { get; set; }

        public string? Title { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime LastMessageAt { get; set; }

        public int MessageCount { get; set; }
    }
}
