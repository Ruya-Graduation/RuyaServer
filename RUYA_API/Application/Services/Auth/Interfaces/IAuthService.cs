using RUYA_API.Application.Services.Auth.DTOs;
using RUYA_API.Responses;

namespace RUYA_API.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<string>> LoginAsync(LoginRequest request);
        Task<ApiResponse<object>> ForgotPassword(ForgotPasswordRequest request);
        Task<ApiResponse<VerifyOtpResponse>> VerifyOtp(VerifyOtpRequest request);
        Task<ApiResponse<object>> ResetPassword(ResetPasswordRequest request);
    }
}
