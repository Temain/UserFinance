using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddUserPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContextPool<UserDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}
