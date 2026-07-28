using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Admin.DTOs.Site
{
    public class UpdateSiteDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        public float Latitude { get; set; }

        [Required]
        public float Longitude { get; set; }
    }
}
