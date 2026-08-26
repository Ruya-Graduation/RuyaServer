using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Admin.DTOs.Site
{
    public class SiteTranslationDto
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
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Hours { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Ticket { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Crowds { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
    }
}
