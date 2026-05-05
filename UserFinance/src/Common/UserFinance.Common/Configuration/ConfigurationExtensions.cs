using Microsoft.Extensions.Configuration;
using UserFinance.Common.Persistence;
using UserFinance.Common.Security;

namespace UserFinance.Common.Configuration;

public static class ConfigurationExtensions
{
    public static PostgresOptions GetPostgresOptions(this IConfiguration configuration)
    {
        return new PostgresOptions
        {
            Host = configuration[EnvironmentVariables.PostgresHost] ?? string.Empty,
            Database = configuration[EnvironmentVariables.PostgresDatabase] ?? string.Empty,
            Username = configuration[EnvironmentVariables.PostgresUsername] ?? string.Empty,
            Password = configuration[EnvironmentVariables.PostgresPassword] ?? string.Empty,
            Port = int.TryParse(configuration[EnvironmentVariables.PostgresPort], out var port) ? port : 0
        };
    }

    public static JwtOptions GetJwtOptions(this IConfiguration configuration)
    {
        return new JwtOptions
        {
            Issuer = configuration[EnvironmentVariables.JwtIssuer] ?? string.Empty,
            Audience = configuration[EnvironmentVariables.JwtAudience] ?? string.Empty,
            SigningKey = configuration[EnvironmentVariables.JwtSigningKey] ?? string.Empty,
            ExpiresInMinutes = int.TryParse(configuration[EnvironmentVariables.JwtExpiresInMinutes], out var minutes)
                ? minutes
                : 0
        };
    }
}
