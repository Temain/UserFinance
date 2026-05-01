using FinanceService.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceService.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddFinanceBusiness(this IServiceCollection services)
    {
        services.AddScoped<IFinanceService, Services.FinanceService>();

        return services;
    }
}
