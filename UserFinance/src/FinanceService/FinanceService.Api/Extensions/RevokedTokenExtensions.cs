using FinanceService.Api.Middleware;

namespace FinanceService.Api.Extensions;

public static class RevokedTokenExtensions
{
    public static IApplicationBuilder UseRevokedTokenValidation(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RevokedTokenMiddleware>();
    }
}
