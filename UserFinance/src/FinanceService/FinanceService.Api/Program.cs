using FinanceService.Api.Extensions;
using FinanceService.Infrastructure;
using FinanceService.Business;
using FinanceService.Application;
using UserFinance.Common.Configuration;
using UserFinance.Common.Extensions;
using UserFinance.Common.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLocalHttpUrl(EnvironmentVariables.FinanceServicePort);

var postgresOptions = builder.Configuration.GetPostgresOptions();
var jwtOptions = builder.Configuration.GetJwtOptions();
var connectionString = PostgresConnectionStringFactory.Create(postgresOptions);

builder.Services.AddOpenApiDocumentation();
builder.Services.AddExceptionHandling();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationId();
builder.Services.AddCurrentUserAccessor();
builder.Services.AddUserServiceOptions(builder.Configuration);
builder.Services.AddFinanceApplication();
builder.Services.AddFinanceBusiness();
builder.Services.AddFinancePersistence(connectionString);
builder.Services.AddUserFavoritesClient();
builder.Services.AddJwtAuthentication(jwtOptions);

var app = builder.Build();

app.UseCorrelationId();
app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapFinanceServiceEndpoints();

app.Run();
