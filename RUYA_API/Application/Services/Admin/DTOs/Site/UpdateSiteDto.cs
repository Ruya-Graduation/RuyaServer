using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Admin.DTOs.Site
{
    public class UpdateSiteDto
    {
        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one translation is required.")]
        public List<SiteTranslationDto> Translations { get; set; } = new();
    }
}
