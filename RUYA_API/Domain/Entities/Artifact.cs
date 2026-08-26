using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Artifact : EntityBase
    {
        public int SiteId { get; set; }
        public Site Site { get; set; } = null!;

        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;

        public ICollection<ArtifactTranslation> Translations { get; set; } = new List<ArtifactTranslation>();
        public ICollection<TourStop> TourStops { get; set; } = new List<TourStop>();
        public ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();

        // Many-to-many (verified_by): an artifact can be backed by several sources
        public ICollection<Source> Sources { get; set; } = new List<Source>();

        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
    }
}
