using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class RegisterRequest
    {
        /// <summary>
        /// User's full name.
        /// Example: Ahmed Mohamed
        /// </summary>
        [Required(ErrorMessage = "User name is required.")]
        [MaxLength(150)]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string PreferredLanguage { get; set; }

        [Required]
        [MaxLength(50)]
        public string KnowledgeLevel { get; set; }
    }
}
