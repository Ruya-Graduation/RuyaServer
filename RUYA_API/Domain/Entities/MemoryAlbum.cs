using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class MemoryAlbum : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int TourId { get; set; }
        public Tour Tour { get; set; } = null!;

        public DateTime GeneratedAt { get; set; }
        public string SummaryText { get; set; } = string.Empty;

        public ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();
    }
}
