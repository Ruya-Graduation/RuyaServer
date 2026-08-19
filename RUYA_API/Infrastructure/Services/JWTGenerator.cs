using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RUYA_API.Application.Common.Interfaces;
using RUYA_API.Domain.Entities;
using RUYA_API.Infrastructure.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RUYA_API.Infrastructure.Services
{
    public class JWTGenerator : IJWTGenerator
    {
        private readonly JwtSettings _jwtSettings;

        // Inject IOptions to get the strongly typed settings
        public JWTGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim("PreferredLanguage", user.PreferredLanguage ?? "en"),
            new Claim("KnowledgeLevel", user.KnowledgeLevel ?? "beginner")
        };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
