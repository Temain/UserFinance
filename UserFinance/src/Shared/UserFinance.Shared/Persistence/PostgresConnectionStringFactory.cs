using Npgsql;

namespace UserFinance.Shared.Persistence;

public static class PostgresConnectionStringFactory
{
    public static string Create(PostgresOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Host))
        {
            throw new InvalidOperationException("Postgres host is not configured.");
        }

        if (options.Port <= 0)
        {
            throw new InvalidOperationException("Postgres port is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.Database))
        {
            throw new InvalidOperationException("Postgres database is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.Username))
        {
            throw new InvalidOperationException("Postgres username is not configured.");
        }

        return new NpgsqlConnectionStringBuilder
        {
            Host = options.Host,
            Port = options.Port,
            Database = options.Database,
            Username = options.Username,
            Password = options.Password
        }.ConnectionString;
    }
}
