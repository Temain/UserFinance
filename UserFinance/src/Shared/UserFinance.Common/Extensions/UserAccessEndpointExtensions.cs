using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using UserFinance.Common.Security;

namespace UserFinance.Common.Extensions;

public static class UserAccessEndpointExtensions
{
    public static RouteHandlerBuilder RequireCurrentUserAccess(this RouteHandlerBuilder builder,
        string routeParameterName = "userId")
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var currentUserAccessor = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserAccessor>();
            var routeValue = context.HttpContext.Request.RouteValues[routeParameterName]?.ToString();

            if (!long.TryParse(routeValue, out var userId) || !currentUserAccessor.HasAccessTo(userId))
            {
                return Results.Forbid();
            }

            return await next(context);
        });

        return builder;
    }
}
