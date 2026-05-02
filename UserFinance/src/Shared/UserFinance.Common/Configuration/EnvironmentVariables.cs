namespace UserFinance.Common.Configuration;

public static class EnvironmentVariables
{
    public const string PostgresHost = "POSTGRES_HOST";
    public const string PostgresDatabase = "POSTGRES_DB";
    public const string PostgresUsername = "POSTGRES_USER";
    public const string PostgresPassword = "POSTGRES_PASSWORD";
    public const string PostgresPort = "POSTGRES_PORT";
    public const string JwtIssuer = "JWT_ISSUER";
    public const string JwtAudience = "JWT_AUDIENCE";
    public const string JwtSigningKey = "JWT_SIGNING_KEY";
    public const string JwtExpiresInMinutes = "JWT_EXPIRES_IN_MINUTES";
}
