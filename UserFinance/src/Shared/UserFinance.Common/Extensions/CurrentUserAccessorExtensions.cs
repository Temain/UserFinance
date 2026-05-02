using Microsoft.Extensions.DependencyInjection;
using UserFinance.Common.Security;

namespace UserFinance.Common.Extensions;

public static class CurrentUserAccessorExtensions
{
    public static IServiceCollection AddCurrentUserAccessor(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        return services;
    }

    public static bool HasAccessTo(this ICurrentUserAccessor currentUserAccessor, long userId)
    {
        return currentUserAccessor.UserId == userId;
    }
}
