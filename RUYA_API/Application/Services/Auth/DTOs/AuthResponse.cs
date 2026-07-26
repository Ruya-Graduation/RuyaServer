namespace RUYA_API.Application.Services.Auth.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
