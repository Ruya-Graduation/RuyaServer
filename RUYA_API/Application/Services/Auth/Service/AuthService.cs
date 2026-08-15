using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RUYA_API.Application.Common;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Application.Services.Auth.DTOs;
using RUYA_API.Application.Services.Auth.Enums;
using RUYA_API.Application.Services.Auth.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.ExceptionHandling.Common;
using RUYA_API.ExceptionHandling.CustomException;
using RUYA_API.Responses;

namespace RUYA_API.Application.Services.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJWTGenerator _jwtGenerator;
        private readonly IOtpService _otpService;
        private readonly IEmailSender _emailSender;

        // Simple constructor injection. No MediatR.
        public AuthService(IUserService userService, IJWTGenerator jwtGenerator, IOtpService otpService,
        IEmailSender emailSender)
        {
            _userService = userService;
            _jwtGenerator = jwtGenerator;
            _otpService = otpService;
            _emailSender = emailSender;
        }
        public async Task<ApiResponse<string>> LoginAsync(LoginRequest request)
        {
            // 1. Find the user by email
            var user = await _userService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new AppException("Invalid email or password.", 400);
            }

            // 2. Verify the password (uses Identity's PasswordHasher)
            var isPasswordValid = await _userService.CheckPasswordAsync(user.Id, request.Password);
            if (!isPasswordValid)
            {
                throw new AppException("Invalid email or password.",400);
            }

            // 3. Get roles and Generate JWT
            var roles = await _userService.GetRolesAsync(user.Id);
            var token = _jwtGenerator.GenerateToken(user, roles);

            // 4. Return the response
            return ResponseFactory.Success<string>(token, "user logged in");
        }

        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            // 1. Check if user already exists (Domain business rule)
            var existingUser = await _userService.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new AppException("A user with this email already exists.",400);
            }

            // 2. Create the Domain Entity (Plain POCO)
            var user = new User
            {
                Id = Guid.NewGuid().ToString(), // Identity expects string ID
                UserName = request.UserName,       // Identity uses this for login
                Email = request.Email,
                FullName = request.UserName,
                PreferredLanguage = request.PreferredLanguage,
                KnowledgeLevel = request.KnowledgeLevel
            };

            // 3. Save to the database (via Infrastructure wrapper)
            var createResult = await _userService.CreateUserAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors);
                throw new AppException($"Registration failed: {errors}", 400);
            }

            var roleResult = await _userService.AssignRoleAsync(user.Id, Roles.User);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors);
                throw new AppException($"Role assignment failed: {errors}", 500);
            }
            // 4. Get roles (if any) and Generate JWT
            var roles = await _userService.GetRolesAsync(user.Id); // Empty if no roles set
            var token = _jwtGenerator.GenerateToken(user, roles);

            // 5. Return the response
            return ResponseFactory.Success<string>(token, "user registered successfully");
        }

        
        public async Task<ApiResponse<object>> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = await _userService.FindByEmailAsync(request.Email);

            // Same response whether or not the account exists / is confirmed.
            if (user is not null)
            {
                await _emailSender.SendOtpEmailAsync(request.Email, _otpService.GenerateAndStoreOtp(request.Email));
            }
            return ResponseFactory.Success("If an account with that email exists, a verification code has been sent.");
        }

        public async Task<ApiResponse<VerifyOtpResponse>> VerifyOtp(VerifyOtpRequest request)
        {
            var result = _otpService.VerifyOtp(request.Email, request.Code);
            if (result == OtpVerificationResult.TooManyAttempts)
                throw new AppException("Too many attempts. Please try again later.", 429);

            if (result != OtpVerificationResult.Success)
                throw new AppException("Invalid or expired verification code.", 400);

            return ResponseFactory.Success<VerifyOtpResponse>(new VerifyOtpResponse {
                ResetToken = _otpService.IssueResetToken(request.Email),
                ExpiresInSeconds = 300
            }, "verified");
            
        }

        public async Task<ApiResponse<object>> ResetPassword(ResetPasswordRequest request)
        {
            var email = _otpService.ConsumeResetToken(request.Email, request.ResetToken);
            if (email is null)
                throw new AppException("Reset session is invalid or has expired. Please start over.", 400);

            var user = await _userService.FindByEmailAsync(email);
            if (user is null)
                throw new AppException("Reset session is invalid or has expired. Please start over.", 400);

            // Generated and consumed server-side only - client never sees this token.
            var identityToken = await _userService.GeneratePasswordResetTokenAsync(user);
            var result = await _userService.ResetPasswordAsync(user, identityToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors);
                return ResponseFactory.Failure($"Could not reset password: {errors}", result.Errors.ToList());
            }

            return ResponseFactory.Success("Password has been reset successfully.");
        }
    }
}
