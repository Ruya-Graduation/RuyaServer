using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Message : EntityBase
    {
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;

        public string Sender { get; set; } = string.Empty;
        public string AgentType { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
