using ApiGateway.Extensions;
using UserFinance.Common.Configuration;
using UserFinance.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLocalHttpUrl(EnvironmentVariables.ApiGatewayPort);

var jwtOptions = builder.Configuration.GetJwtOptions();
var userServiceUrl = builder.Configuration[EnvironmentVariables.UserServiceUrl] ?? string.Empty;
var financeServiceUrl = builder.Configuration[EnvironmentVariables.FinanceServiceUrl] ?? string.Empty;

builder.Services.AddApiGatewaySwagger();
builder.Services.AddJwtAuthentication(jwtOptions);
builder.Services.AddApiGatewayReverseProxy(userServiceUrl, financeServiceUrl);

var app = builder.Build();

app.UseApiGatewaySwagger();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy"
}));
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapReverseProxy();

app.Run();
