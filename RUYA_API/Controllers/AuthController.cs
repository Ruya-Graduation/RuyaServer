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
            // [ApiController] automatically validates the request (DataAnnotations)
            // and returns a 400 Bad Request if invalid.

            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
    }
}
