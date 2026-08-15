namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class VerifyOtpResponse
    {
        // Opaque one-time token the client must send back in step 3.
        // Not the same as Identity's internal password reset token — the client never sees that one.
        public string ResetToken { get; set; } = string.Empty;
        public int ExpiresInSeconds { get; set; }
    }
}
