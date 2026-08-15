using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class ConversationAttachment : EntityBase
    {
        public int ConversationId { get; set; }

        public Conversation Conversation { get; set; } = null!;

        public int? MessageId { get; set; }

        public Message? Message { get; set; }

        public string FileUrl { get; set; } = string.Empty;

        public string? PublicId { get; set; }

        public string FileType { get; set; } = string.Empty;

        public string MimeType { get; set; } = string.Empty;

        public bool IsPrimaryFrame { get; set; }

        public string? VisionResultJson { get; set; }
    }
}
