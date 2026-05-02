using CurrencyUpdater.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace CurrencyUpdater.Application.Services;

public sealed class CurrencyService(
    ICurrencyRatesProvider currencyRatesProvider,
    ICurrencyRatesParser currencyRatesParser,
    ICurrencyWriteRepository currencyWriteRepository,
    ILogger<CurrencyService> logger) : ICurrencyService
{
    public async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching daily currency rates from CBR.");

        var xml = await currencyRatesProvider.GetDailyRatesAsync(cancellationToken);
        var currencyRates = currencyRatesParser.Parse(xml);

        logger.LogInformation("Parsed {CurrencyCount} currency rates.", currencyRates.Count);

        await currencyWriteRepository.UpsertAsync(currencyRates, cancellationToken);

        logger.LogInformation("Currency rates updated successfully.");
    }
}
