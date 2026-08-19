using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.MemoryAlbum.DTOs
{
    public class CreateMemoryAlbumDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [MaxLength(500)]
        public string CoverImage { get; set; } = string.Empty;

        public List<CreateAlbumItemDto> AlbumItems { get; set; } = new();
    }
}
