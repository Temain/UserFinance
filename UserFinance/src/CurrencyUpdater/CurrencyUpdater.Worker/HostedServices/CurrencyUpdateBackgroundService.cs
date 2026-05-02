using CurrencyUpdater.Application.Services;
using CurrencyUpdater.Worker.Options;
using Microsoft.Extensions.Options;

namespace CurrencyUpdater.Worker.HostedServices;

public sealed class CurrencyUpdateBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<CurrencyUpdaterOptions> currencyUpdaterOptions,
    ILogger<CurrencyUpdateBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await UpdateAsync(stoppingToken);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(currencyUpdaterOptions.Value.UpdateIntervalMinutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await UpdateAsync(stoppingToken);
        }
    }

    private async Task UpdateAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = serviceScopeFactory.CreateScope();
            var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
            await currencyService.UpdateAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Currency updater is stopping.");
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Currency updater failed to refresh exchange rates.");
        }
    }
}
