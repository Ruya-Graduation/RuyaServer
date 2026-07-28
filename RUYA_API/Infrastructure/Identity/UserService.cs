using Microsoft.AspNetCore.Identity;
using RUYA_API.Application.Common.DTOs;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUYA_API.Infrastructure.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null ? null : user;
        }

        public async Task<IdentityOperationResult> CreateUserAsync(User newUser,string password)
        {
            var result = await _userManager.CreateAsync(newUser,password);

            return result.Succeeded
                ? IdentityOperationResult.Success()
                : IdentityOperationResult.Failure(result.Errors.Select(e => e.Description));
        }

        public async Task<bool> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IEnumerable<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return Array.Empty<string>();

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityOperationResult> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return IdentityOperationResult.Failure(new[] { "User not found." });
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded
                ? IdentityOperationResult.Success()
                : IdentityOperationResult.Failure(result.Errors.Select(e => e.Description));
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            var result = await _userManager.GeneratePasswordResetTokenAsync(user);
            return result;
        }

        public async Task<IdentityOperationResult> ResetPasswordAsync(User user, string token, string newPass)
        {
            var result = await _userManager.ResetPasswordAsync(user, token, newPass);
            return result.Succeeded
                ? IdentityOperationResult.Success()
                : IdentityOperationResult.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
