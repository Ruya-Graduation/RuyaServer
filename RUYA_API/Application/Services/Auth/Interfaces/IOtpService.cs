using RUYA_API.Application.Services.Auth.Enums;

namespace RUYA_API.Application.Services.Auth.Interfaces
{
    public interface IOtpService
    {
        string GenerateAndStoreOtp(string email);
        OtpVerificationResult VerifyOtp(string email, string code);
        string IssueResetToken(string email);
        string? ConsumeResetToken(string email, string resetToken);
    }
}
