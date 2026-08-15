namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class OtpEntry
    {
        public string CodeHash { get; set; } = string.Empty;
        public int Attempts { get; set; }
        public DateTimeOffset ExpiresAtUtc { get; set; }
    }
}
