using CurrencyUpdater.Application.Abstractions;

namespace CurrencyUpdater.Tests.Fakes;

internal sealed class FakeCurrencyRatesProvider(string xml) : ICurrencyRatesProvider
{
    public Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(xml);
    }
}
