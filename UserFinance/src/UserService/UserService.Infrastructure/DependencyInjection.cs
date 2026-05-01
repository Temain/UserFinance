using UserService.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUserPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<UserDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly("UserService.Migrations")
                    .MigrationsHistoryTable("__EFMigrationsHistory_User")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserCurrencyRepository, UserCurrencyRepository>();

        return services;
    }
}
