using UserFinance.Shared.Configuration;
using UserFinance.Shared.Persistence;
using UserService.Api.Extensions;
using UserService.Application;
using UserService.Business;
using UserService.Infrastructure;
using UserService.Infrastructure.Options;

var builder = WebApplication.CreateBuilder(args);

var postgresOptions = CreatePostgresOptions(builder.Configuration);
var jwtOptions = CreateJwtOptions(builder.Configuration);
var connectionString = PostgresConnectionStringFactory.Create(postgresOptions);

builder.Services.AddOpenApiDocumentation();
builder.Services.AddExceptionHandling();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCurrentUserAccessor();
builder.Services.AddUserApplication();
builder.Services.AddUserBusiness();
builder.Services.AddUserPersistence(connectionString);
builder.Services.AddUserSecurity(jwtOptions);
builder.Services.AddJwtAuthentication(jwtOptions);

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserServiceEndpoints();

app.Run();

static PostgresOptions CreatePostgresOptions(ConfigurationManager configuration)
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

static JwtOptions CreateJwtOptions(ConfigurationManager configuration)
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
