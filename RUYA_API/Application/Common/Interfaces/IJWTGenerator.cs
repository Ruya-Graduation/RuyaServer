using RUYA_API.Domain.Entities;

namespace RUYA_API.Application.Common.Interfaces
{
    public interface IJWTGenerator
    {
        string GenerateToken(User user, IEnumerable<string>roles); 
    }
}
