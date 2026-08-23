using RUYA_API.Domain.Common;

namespace RUYA_API.Domain.Entities
{
    public class AlbumItem : EntityBase
    {
        public int AlbumId { get; set; }
        public MemoryAlbum MemoryAlbum { get; set; } = null!;

        public int? ArtifactId { get; set; }
        public Artifact? Artifact { get; set; }

        public string PhotoUrl { get; set; } = string.Empty;
        public string? PublicId { get; set; }
        public string? Caption { get; set; }
        public string? DayLabel { get; set; }
        public string? AiSummary { get; set; }
    }
}
