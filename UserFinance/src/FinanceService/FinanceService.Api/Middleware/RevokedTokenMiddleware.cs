using FinanceService.Abstractions.Repositories;
using UserFinance.Common.Security;

namespace FinanceService.Api.Middleware;

public sealed class RevokedTokenMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, ICurrentUserAccessor currentUserAccessor,
        IRevokedTokenRepository revokedTokenRepository)
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

        if (await revokedTokenRepository.ExistsAsync(currentUserAccessor.JwtId, httpContext.RequestAborted))
        {
            httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await next(httpContext);
    }
}
