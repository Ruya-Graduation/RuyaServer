using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RUYA_API.Application.Services.Auth.Interfaces;

namespace RUYA_API.Application.Services.Auth.Service
{
    public class EmailSender: IEmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> settings, ILogger<EmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otpCode, CancellationToken ct = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Your password reset code";

            message.Body = new TextPart("plain")
            {
                Text = $"Your verification code is: {otpCode}\n\nThis code expires in 10 minutes. " +
                       "If you didn't request this, you can ignore this email."
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls, ct);
                await client.AuthenticateAsync(_settings.SenderEmail, _settings.AppPassword, ct);
                await client.SendAsync(message, ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send OTP email to {Email}", toEmail);
                throw; // let AuthController's catch block handle it, same as before
            }
            finally
            {
                if (client.IsConnected)
                    await client.DisconnectAsync(true, ct);
            }
        }
    }


    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;
    }
}
