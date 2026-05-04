using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserFinance.Common.Security;
using UserService.Abstractions.Security;

namespace UserService.Infrastructure.Security;

public sealed class JwtTokenGenerator(JwtOptions jwtOptions) : IJwtTokenGenerator
{
    public string GenerateToken(long userId, string userName)
    {
        ValidateOptions(jwtOptions);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtOptions.ExpiresInMinutes);

        var token = new JwtSecurityToken(jwtOptions.Issuer, jwtOptions.Audience, claims, expires: expiresAt,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void ValidateOptions(JwtOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            throw new InvalidOperationException("JWT issuer is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            throw new InvalidOperationException("JWT audience is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.SigningKey))
        {
            throw new InvalidOperationException("JWT signing key is not configured.");
        }

        if (options.ExpiresInMinutes <= 0)
        {
            throw new InvalidOperationException("JWT expiration time must be greater than zero.");
        }
    }
}
