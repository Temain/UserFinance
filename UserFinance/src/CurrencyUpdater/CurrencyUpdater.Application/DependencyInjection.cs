using CurrencyUpdater.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyUpdater.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrencyUpdaterApplication(this IServiceCollection services)
    {
        services.AddScoped<ICurrencyService, CurrencyService>();
        return services;
    }
}
