using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class TourStop : EntityBase
    {
        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;

        public int ArtifactId { get; set; }
        public Artifact Artifact { get; set; } = null!;

        public DateTime VisitedAt { get; set; }

        //public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
