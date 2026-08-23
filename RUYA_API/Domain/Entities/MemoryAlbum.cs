using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class MemoryAlbum : EntityBase
    {
        public string UserId { get; set; }
        public User User { get; set; } = null!;

        public int? TourId { get; set; }
        public Tour? Tour { get; set; }

        public string Title { get; set; } = string.Empty;
        public string CoverImage { get; set; } = string.Empty;
        public string StartDate { get; set; } = string.Empty;
        public string? SummaryText { get; set; }

        public ICollection<AlbumItem> AlbumItems { get; set; } = new List<AlbumItem>();
    }
}
