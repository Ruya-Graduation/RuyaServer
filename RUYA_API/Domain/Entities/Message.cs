using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Message : EntityBase
    {
        public int ConversationId { get; set; }

        public Conversation Conversation { get; set; } = null!;

        // user | assistant | system
        public string Role { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int? InputTokens { get; set; }

        public int? OutputTokens { get; set; }

        public string? ModelName { get; set; }

        public string? Metadata { get; set; }
    }
}
