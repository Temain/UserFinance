using CurrencyUpdater.Application.Models;

namespace CurrencyUpdater.Application.Abstractions;

public interface ICurrencyWriteRepository
{
    Task UpsertAsync(IReadOnlyCollection<CurrencyRateDto> currencyRates, CancellationToken cancellationToken = default);
}
