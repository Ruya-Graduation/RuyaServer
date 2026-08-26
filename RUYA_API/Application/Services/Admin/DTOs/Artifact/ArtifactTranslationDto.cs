using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Admin.DTOs.Artifact
{
    public class ArtifactTranslationDto
    {
        [Required]
        [MaxLength(2)]
        [RegularExpression("^(ar|en)$", ErrorMessage = "Language code must be 'ar' or 'en'.")]
        public string LanguageCode { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Civilization { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Period { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Material { get; set; } = string.Empty;

        [MaxLength(200)]
        public string PlaceOfDiscovery { get; set; } = string.Empty;
    }
}
