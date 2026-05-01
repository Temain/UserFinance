using FinanceService.Infrastructure;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserFinance.Shared.Configuration;
using UserFinance.Shared.Persistence;
using UserService.Infrastructure;
using UserService.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<PostgresOptions>(options =>
{
    options.Host = builder.Configuration[EnvironmentVariables.PostgresHost] ?? string.Empty;
    options.Database = builder.Configuration[EnvironmentVariables.PostgresDatabase] ?? string.Empty;
    options.Username = builder.Configuration[EnvironmentVariables.PostgresUsername] ?? string.Empty;
    options.Password = builder.Configuration[EnvironmentVariables.PostgresPassword] ?? string.Empty;
    options.Port = int.TryParse(builder.Configuration[EnvironmentVariables.PostgresPort], out var port) ? port : 0;
});

await using var serviceProvider = builder.Services.BuildServiceProvider();
var postgresOptions = serviceProvider.GetRequiredService<IOptions<PostgresOptions>>().Value;
var connectionString = PostgresConnectionStringFactory.Create(postgresOptions);

builder.Services.AddUserPersistence(connectionString);
builder.Services.AddFinancePersistence(connectionString);

using var host = builder.Build();
using var scope = host.Services.CreateScope();

var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("MigrationService");
var userDbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
var financeDbContext = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();

logger.LogInformation("Applying migrations for UserDbContext.");
await userDbContext.Database.MigrateAsync();

logger.LogInformation("Applying migrations for FinanceDbContext.");
await financeDbContext.Database.MigrateAsync();

logger.LogInformation("Database migrations completed successfully.");
