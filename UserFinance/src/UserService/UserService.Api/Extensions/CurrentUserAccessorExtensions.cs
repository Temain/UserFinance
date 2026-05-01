using UserService.Application.Abstractions.Services;
using UserService.Api.Services;

namespace UserService.Api.Extensions;

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
