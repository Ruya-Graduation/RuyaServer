using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class Artifact : EntityBase
    {
        public int SiteId { get; set; }
        public Site Site { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Civilization { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePublicId { get; set; } = string.Empty;

        public ICollection<TourStop> TourStops { get; set; } = new List<TourStop>();
        public ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();

        // Many-to-many (verified_by): an artifact can be backed by several sources
        public ICollection<Source> Sources { get; set; } = new List<Source>();
    }
}
