using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Application.Metrics;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace CurrencyUpdater.Application.Services;

public sealed class CurrencyService(
    ICurrencyRatesProvider currencyRatesProvider,
    ICurrencyRatesParser currencyRatesParser,
    ICurrencyWriteRepository currencyWriteRepository,
    ILogger<CurrencyService> logger) : ICurrencyService
{
    public async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        using var updateTimer = CurrencyUpdaterMetrics.UpdateDuration.NewTimer();
        CurrencyUpdaterMetrics.UpdateRuns.Inc();
        logger.LogInformation("Fetching daily currency rates from CBR.");

        var xml = await currencyRatesProvider.GetDailyRatesAsync(cancellationToken);
        var currencyRates = currencyRatesParser.Parse(xml);
        var currencyIds = currencyRates.Select(currency => currency.CurrencyId).ToArray();
        var existingCurrencies = await currencyWriteRepository.GetByIdsAsync(currencyIds, cancellationToken);
        var newCurrencies = currencyRates
            .Where(currency => !existingCurrencies.ContainsKey(currency.CurrencyId))
            .ToArray();
        var updatedCurrencies = currencyRates
            .Where(currency => existingCurrencies.ContainsKey(currency.CurrencyId))
            .ToArray();

        logger.LogInformation("Parsed {CurrencyCount} currency rates.", currencyRates.Count);

        if (newCurrencies.Length > 0)
        {
            currencyWriteRepository.AddRange(newCurrencies);
        }

        if (updatedCurrencies.Length > 0)
        {
            currencyWriteRepository.UpdateRange(updatedCurrencies);
        }

        await currencyWriteRepository.SaveChangesAsync(cancellationToken);

        CurrencyUpdaterMetrics.UpdatedCurrencies.Inc(currencyRates.Count);
        CurrencyUpdaterMetrics.LastSuccessfulUpdate.SetToCurrentTimeUtc();
        logger.LogInformation("Currency rates updated successfully.");
    }
}
