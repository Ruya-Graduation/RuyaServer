using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Moments.DTOs
{
    public class UpdateMomentAlbumDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(50)]
        public string? StartDate { get; set; }
    }
}
