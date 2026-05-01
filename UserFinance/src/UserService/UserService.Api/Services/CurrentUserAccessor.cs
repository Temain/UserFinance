using System.Security.Claims;
using UserService.Application.Abstractions.Services;

namespace UserService.Api.Services;

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
}
