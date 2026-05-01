using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddFinancePersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<FinanceDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly("FinanceService.Migrations")
                    .MigrationsHistoryTable("__EFMigrationsHistory_Finance")));

        return services;
    }
}
