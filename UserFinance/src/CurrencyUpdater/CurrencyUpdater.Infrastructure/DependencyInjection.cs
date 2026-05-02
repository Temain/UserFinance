using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Infrastructure.Integrations;
using CurrencyUpdater.Infrastructure.Persistence;
using CurrencyUpdater.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyUpdater.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCurrencyUpdaterInfrastructure(this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContextPool<CurrencyUpdateDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<ICurrencyWriteRepository, CurrencyWriteRepository>();
        services.AddScoped<ICurrencyRatesParser, CbrCurrencyRatesXmlParser>();
        services.AddHttpClient<ICurrencyRatesProvider, CbrCurrencyRatesProvider>();

        return services;
    }
}
