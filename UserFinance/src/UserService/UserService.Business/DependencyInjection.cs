using Microsoft.Extensions.DependencyInjection;
using UserService.Abstractions.Services;
using UserService.Business.Services;

namespace UserService.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddUserBusiness(this IServiceCollection services)
    {
        services.AddScoped<IUserAuthService, UserAuthService>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        return services;
    }
}
