using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using UserFinance.Configuration;

namespace FinanceService.Migrations;

public sealed class FinanceDbContextFactory : IDesignTimeDbContextFactory<FinanceDbContext>
{
    public FinanceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
        var connectionString = CreateConnectionString();

        optionsBuilder.UseNpgsql(connectionString, npgsql =>
            npgsql.MigrationsAssembly("FinanceService.Migrations")
                .MigrationsHistoryTable("__EFMigrationsHistory_Finance"));

        return new FinanceDbContext(optionsBuilder.Options);
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
