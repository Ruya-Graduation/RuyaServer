using RUYA_API.Domain.Common;
using RUYA_API.Domain.Enums;

namespace RUYA_API.Domain.Entities
{
    public class Tour : EntityBase
    {
        public string UserId { get; set; }
        public User User { get; set; } = null!;

        public int SiteId { get; set; }
        public Site Site { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public TourStatus Status { get; set; }

        public ICollection<TourStop> TourStops { get; set; } = new List<TourStop>();
        public ICollection<MemoryAlbum> MemoryAlbums { get; set; } = new List<MemoryAlbum>();
    }
}
