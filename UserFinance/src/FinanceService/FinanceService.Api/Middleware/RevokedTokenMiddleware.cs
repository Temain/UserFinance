using FinanceService.Abstractions.Integrations;
using UserFinance.Common.Security;

namespace FinanceService.Api.Middleware;

public sealed class RevokedTokenMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, ICurrentUserAccessor currentUserAccessor,
        IRevokedTokenClient revokedTokenClient)
    {
        if (httpContext.User.Identity?.IsAuthenticated != true)
        {
            await next(httpContext);
            return;
        }

        if (string.IsNullOrWhiteSpace(currentUserAccessor.JwtId))
        {
            await next(httpContext);
            return;
        }

        if (await revokedTokenClient.IsRevokedAsync(currentUserAccessor.JwtId, httpContext.RequestAborted))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(httpContext);
    }
}
