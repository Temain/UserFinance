using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace CurrencyUpdater.Infrastructure.Integrations;

public sealed class CbrCurrencyRatesProvider(HttpClient httpClient, IOptions<CbrOptions> cbrOptions)
    : ICurrencyRatesProvider
{
    public Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default)
    {
        return httpClient.GetStringAsync(cbrOptions.Value.DailyRatesUrl, cancellationToken);
    }
}
