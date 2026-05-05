using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Http;

namespace UserFinance.Common.Security;

public sealed class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public long? UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name)
                ?? httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

            return long.TryParse(value, out var userId) ? userId : null;
        }
    }

    public string? AccessToken
    {
        get
        {
            var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (string.IsNullOrWhiteSpace(authorizationHeader))
            {
                return null;
            }

            const string bearerPrefix = "Bearer ";
            return authorizationHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
                ? authorizationHeader[bearerPrefix.Length..].Trim()
                : authorizationHeader;
        }
    }

    public string? JwtId => httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Jti)
        ?? httpContextAccessor.HttpContext?.User.FindFirstValue("jti");

    public DateTimeOffset? ExpiresAtUtc
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Exp)
                ?? httpContextAccessor.HttpContext?.User.FindFirstValue("exp");

            if (!long.TryParse(value, out var unixTimeSeconds))
            {
                return null;
            }

            return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds);
        }
    }
}
