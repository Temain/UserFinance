using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using UserFinance.Configuration;
using UserService.Infrastructure.Persistence;

namespace UserService.Migrations;

public sealed class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        var connectionString = CreateConnectionString();

        optionsBuilder.UseNpgsql(connectionString, npgsql =>
            npgsql.MigrationsAssembly("UserService.Migrations")
                .MigrationsHistoryTable("__EFMigrationsHistory_User"));

        return new UserDbContext(optionsBuilder.Options);
    }

    private static string CreateConnectionString()
    {
        var host = GetRequiredEnvironmentVariable(EnvironmentVariables.PostgresHost);
        var database = GetRequiredEnvironmentVariable(EnvironmentVariables.PostgresDatabase);
        var username = GetRequiredEnvironmentVariable(EnvironmentVariables.PostgresUsername);
        var password = GetRequiredEnvironmentVariable(EnvironmentVariables.PostgresPassword);
        var portValue = GetRequiredEnvironmentVariable(EnvironmentVariables.PostgresPort);

        if (!int.TryParse(portValue, out var port) || port <= 0)
        {
            throw new InvalidOperationException(
                $"Environment variable '{EnvironmentVariables.PostgresPort}' is not configured correctly.");
        }

        return new NpgsqlConnectionStringBuilder
        {
            Host = host,
            Port = port,
            Database = database,
            Username = username,
            Password = password
        }.ConnectionString;
    }

    private static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Environment variable '{variableName}' is not configured.");
        }

        return value;
    }
}
