using FinanceService.Abstractions.Integrations;
using FinanceService.Abstractions.Repositories;
using FinanceService.Infrastructure.Integrations;
using FinanceService.Infrastructure.Options;
using FinanceService.Infrastructure.Persistence;
using FinanceService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace FinanceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancePersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<FinanceDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly("FinanceService.Migrations")
                    .MigrationsHistoryTable("__EFMigrationsHistory_Finance")));

        services.AddScoped<ICurrencyRepository, CurrencyRepository>();

        return services;
    }

    public static IServiceCollection AddUserFavoritesClient(this IServiceCollection services)
    {
        services.AddHttpClient<IUserFavoritesClient, UserFavoritesHttpClient>((serviceProvider, client) =>
            {
                var userServiceOptions = serviceProvider.GetRequiredService<IOptions<UserServiceOptions>>().Value;
                client.BaseAddress = new Uri(userServiceOptions.BaseUrl);
            })
            .AddPolicyHandler((serviceProvider, _) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<UserFavoritesHttpClient>>();

                return HttpPolicyExtensions.HandleTransientHttpError()
                    .Or<TaskCanceledException>()
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        onRetry: (outcome, delay, retryAttempt, _) =>
                        {
                            logger.LogWarning(
                                "Retrying request to user service. Attempt {RetryAttempt} after {DelaySeconds} seconds.",
                                retryAttempt, delay.TotalSeconds);
                        });
            });

        return services;
    }
}
