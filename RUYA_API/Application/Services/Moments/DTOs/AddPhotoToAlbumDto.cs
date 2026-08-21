using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Moments.DTOs
{
    public class AddPhotoToAlbumDto
    {
        [Required]
        public IFormFile Photo { get; set; } = null!;

        public string? Caption { get; set; }

        public string? DayLabel { get; set; }
    }
}
