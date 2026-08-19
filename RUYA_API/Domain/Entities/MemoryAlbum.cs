using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class MemoryAlbum : EntityBase
    {
        public string UserId { get; set; }
        public User User { get; set; } = null!;

        public int? TourId { get; set; }
        public Tour? Tour { get; set; }

        public DateTime GeneratedAt { get; set; }
        public string SummaryText { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string CoverImage { get; set; } = string.Empty;

        public ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();
    }
}
