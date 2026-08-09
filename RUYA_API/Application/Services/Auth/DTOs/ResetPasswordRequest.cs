using System.ComponentModel.DataAnnotations;

namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reset token is required.")]
        public string ResetToken { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "New password and confirm password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
