namespace RUYA_API.Application.Services.Auth.Interfaces
{
    public interface IEmailSender
    {
        Task SendOtpEmailAsync(string toEmail, string otpCode, CancellationToken ct = default);
    }
}
