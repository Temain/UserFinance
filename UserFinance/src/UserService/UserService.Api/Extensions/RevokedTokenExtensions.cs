using UserService.Api.Middleware;

namespace UserService.Api.Extensions;

public static class RevokedTokenExtensions
{
    public static IApplicationBuilder UseRevokedTokenValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RevokedTokenMiddleware>();
    }
}
