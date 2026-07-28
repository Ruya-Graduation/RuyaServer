namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class VerifyOtpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
