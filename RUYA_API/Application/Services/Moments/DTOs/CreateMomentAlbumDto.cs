using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Moments.DTOs
{
    public class CreateMomentAlbumDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string StartDate { get; set; } = string.Empty;

        public IFormFile? CoverPhoto { get; set; }
    }
}
