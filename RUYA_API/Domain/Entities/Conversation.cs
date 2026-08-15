using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Conversation : EntityBase
    {
        public string? UserId { get; set; }

        public User? User { get; set; }

        public string? Title { get; set; }

        public string Status { get; set; } = "Active";

        public int? CurrentArtifactId { get; set; }

        public Artifact? CurrentArtifact { get; set; }

        public string CurrentLanguage { get; set; } = "en";

        public string CurrentMode { get; set; } = "story";

        public DateTime LastMessageAt { get; set; }

        public string? Summary { get; set; }

        public string? ModelName { get; set; }

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<ConversationAttachment> Attachments { get; set; } = new List<ConversationAttachment>();
    }
}
