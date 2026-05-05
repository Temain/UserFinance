using FinanceService.Infrastructure.Options;
using UserFinance.Common.Configuration;

namespace FinanceService.Api.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddUserServiceOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<UserServiceOptions>(options =>
        {
            options.BaseUrl = configuration[EnvironmentVariables.UserServiceUrl] ?? string.Empty;

            options.UserFavoritesPath = configuration[EnvironmentVariables.UserServiceFavoritesPath]
                ?? UserServiceOptions.DefaultUserFavoritesPath;

            options.RevokedTokenPath = configuration[EnvironmentVariables.UserServiceRevokedTokenPath]
                ?? UserServiceOptions.DefaultRevokedTokenPath;
        });

        return services;
    }
}
