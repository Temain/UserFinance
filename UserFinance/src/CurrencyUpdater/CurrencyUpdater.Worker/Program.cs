using System.Text;
using CurrencyUpdater.Application;
using CurrencyUpdater.Infrastructure;
using CurrencyUpdater.Worker.Extensions;
using CurrencyUpdater.Worker.HostedServices;
using UserFinance.Common.Configuration;
using UserFinance.Common.Persistence;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);
var postgresOptions = builder.Configuration.GetPostgresOptions();
var connectionString = PostgresConnectionStringFactory.Create(postgresOptions);

builder.Services.AddCurrencyUpdaterOptions(builder.Configuration);
builder.Services.AddCurrencyUpdaterApplication();
builder.Services.AddCurrencyUpdaterInfrastructure(connectionString);
builder.Services.AddHostedService<CurrencyUpdateBackgroundService>();

var host = builder.Build();
host.Run();
