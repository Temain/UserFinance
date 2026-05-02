namespace CurrencyUpdater.Application.Abstractions;

public interface ICurrencyRatesProvider
{
    Task<string> GetDailyRatesAsync(CancellationToken cancellationToken = default);
}
