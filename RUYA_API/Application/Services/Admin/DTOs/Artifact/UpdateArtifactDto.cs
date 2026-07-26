using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Admin.DTOs.Artifact
{
    public class UpdateArtifactDto
    {
        [Required]
        public int SiteId { get; set; }

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

        [MaxLength(500)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
