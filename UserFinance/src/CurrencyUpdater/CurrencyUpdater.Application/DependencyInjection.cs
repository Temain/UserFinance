using CurrencyUpdater.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyUpdater.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrencyUpdaterApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped<ICurrencyService, CurrencyService>();
        return services;
    }
}
