using RUYA_API.Application.Common.DTOs;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByEmailAsync(string email);
        Task<IdentityOperationResult> AssignRoleAsync(string userId, string role);
        Task<IdentityOperationResult> CreateUserAsync(User newUser, string password);
        Task<bool> CheckPasswordAsync(string userId, string password);
        Task<IEnumerable<string>> GetRolesAsync(string userId);
        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityOperationResult> ResetPasswordAsync(User user, string token, string newPass);
    }
}
