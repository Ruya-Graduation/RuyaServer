using RUYA_API.Application.Common.DTOs;
using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByEmailAsync(string email);
        Task<IdentityOperationResult> CreateUserAsync(string email, string userName, string password);
        Task<bool> CheckPasswordAsync(string userId, string password);
        Task<IEnumerable<string>> GetRolesAsync(string userId);
    }
}
