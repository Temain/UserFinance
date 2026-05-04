using UserFinance.Common.Configuration;
using UserFinance.Common.Extensions;
using UserFinance.Common.Persistence;
using UserService.Api.Extensions;
using UserService.Application;
using UserService.Business;
using UserService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLocalHttpUrl(EnvironmentVariables.UserServicePort);

var postgresOptions = builder.Configuration.GetPostgresOptions();
var jwtOptions = builder.Configuration.GetJwtOptions();
var connectionString = PostgresConnectionStringFactory.Create(postgresOptions);

builder.Services.AddOpenApiDocumentation();
builder.Services.AddExceptionHandling();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationId();
builder.Services.AddCurrentUserAccessor();
builder.Services.AddRequestValidation();
builder.Services.AddUserApplication();
builder.Services.AddUserBusiness();
builder.Services.AddUserPersistence(connectionString);
builder.Services.AddUserSecurity(jwtOptions);
builder.Services.AddJwtAuthentication(jwtOptions);

var app = builder.Build();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseRevokedTokenValidation();
app.UseAuthorization();

app.MapUserServiceEndpoints();

app.Run();
