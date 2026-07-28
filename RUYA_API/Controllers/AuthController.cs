using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Services.Auth.DTOs;
using RUYA_API.Application.Services.Auth.Interfaces;
using RUYA_API.Responses;

namespace RUYA_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // Public endpoints
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var response = await _authService.ForgotPassword(request);
            return Ok(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> ForgotPassword(VerifyOtpRequest request)
        {
            var response = await _authService.VerifyOtp(request);
            return Ok(response);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var response = await _authService.ResetPassword(request);
            return Ok(response);
        }



    }
}
