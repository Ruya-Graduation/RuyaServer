using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Chat.DTOs
{
    public class ChatRequestDto
    {
        public int? ConversationId { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public string Language { get; set; } = "en";

        public string Mode { get; set; } = "story";

        public IFormFile? Image { get; set; }
    }
}
