using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.MemoryAlbum.DTOs
{
    public class CreateAlbumItemDto
    {
        [Required]
        public int ArtifactId { get; set; }

        [Required]
        [MaxLength(500)]
        public string PhotoUrl { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Label { get; set; } = string.Empty;
    }
}
