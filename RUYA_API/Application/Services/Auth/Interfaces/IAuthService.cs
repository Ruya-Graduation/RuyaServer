using RUYA_API.Application.Services.Auth.DTOs;

namespace RUYA_API.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
    }
}
