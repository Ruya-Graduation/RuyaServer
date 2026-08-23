using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Admin.DTOs.Artifact
{
    public class UpdateArtifactDto
    {
        [Required]
        public int SiteId { get; set; }

        public IFormFile? Image { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one translation is required.")]
        public List<ArtifactTranslationDto> Translations { get; set; } = new();
    }
}
