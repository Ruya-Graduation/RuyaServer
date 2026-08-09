using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class VerifyOtpRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "OTP code is required.")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "OTP code length is invalid.")]
        public string Code { get; set; } = string.Empty;
    }
}
