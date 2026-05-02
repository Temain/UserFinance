using CurrencyUpdater.Application.Models;

namespace CurrencyUpdater.Application.Abstractions;

public interface ICurrencyWriteRepository
{
    Task<IReadOnlyDictionary<int, CurrencyRateDto>> GetByIdsAsync(IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default);

    void AddRange(IEnumerable<CurrencyRateDto> currencyRates);

    void UpdateRange(IEnumerable<CurrencyRateDto> currencyRates);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
