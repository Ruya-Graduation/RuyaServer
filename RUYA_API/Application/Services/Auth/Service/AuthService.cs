using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Auth.DTOs;
using RUYA_API.Application.Services.Auth.Interfaces;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Services.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJWTGenerator _jwtGenerator;

        // Simple constructor injection. No MediatR.
        public AuthService(IUserService userService, IJWTGenerator jwtGenerator)
        {
            _userService = userService;
            _jwtGenerator = jwtGenerator;
        }
        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            // 1. Find the user by email
            var user = await _userService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // 2. Verify the password (uses Identity's PasswordHasher)
            var isPasswordValid = await _userService.CheckPasswordAsync(user.Id, request.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // 3. Get roles and Generate JWT
            var roles = await _userService.GetRolesAsync(user.Id);
            var token = _jwtGenerator.GenerateToken(user, roles);

            // 4. Return the response
            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            // 1. Check if user already exists (Domain business rule)
            var existingUser = await _userService.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            // 2. Create the Domain Entity (Plain POCO)
            var user = new User
            {
                Id = Guid.NewGuid().ToString(), // Identity expects string ID
                UserName = request.Email,       // Identity uses this for login
                Email = request.Email,
                FullName = request.FullName,
                PreferredLanguage = request.PreferredLanguage,
                KnowledgeLevel = request.KnowledgeLevel
            };

            // 3. Save to the database (via Infrastructure wrapper)
            var createResult = await _userService.CreateUserAsync(user.Email, user.UserName,request.Password);
            if (!createResult.Succeeded)
            {
                // Identity returns a list of errors (e.g., weak password, duplicate)
                //var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Registration failed: {createResult.Errors}");
            }

            // 4. Get roles (if any) and Generate JWT
            var roles = await _userService.GetRolesAsync(user.Id); // Empty if no roles set
            var token = _jwtGenerator.GenerateToken(user, roles);

            // 5. Return the response
            return new AuthResponse
            {
                Token = token,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddHours(1) // Configurable
            };
        }
    }
}
