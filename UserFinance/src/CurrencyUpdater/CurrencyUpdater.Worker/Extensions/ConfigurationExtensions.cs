using CurrencyUpdater.Infrastructure.Options;
using CurrencyUpdater.Worker.Options;
using UserFinance.Common.Configuration;

namespace CurrencyUpdater.Worker.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCurrencyUpdaterOptions(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CbrOptions>(options =>
        {
            options.DailyRatesUrl =
                configuration[EnvironmentVariables.CbrDailyRatesUrl] ?? CbrOptions.DefaultDailyRatesUrl;
        });

        services.Configure<CurrencyUpdaterOptions>(options =>
        {
            options.UpdateIntervalMinutes =
                int.TryParse(configuration[EnvironmentVariables.CurrencyUpdaterIntervalMinutes], out var minutes)
                    ? minutes
                    : CurrencyUpdaterOptions.DefaultUpdateIntervalMinutes;
        });

        return services;
    }
}
