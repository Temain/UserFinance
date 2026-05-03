using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Application.Models;

namespace CurrencyUpdater.Tests.Fakes;

internal sealed class FakeCurrencyWriteRepository(IReadOnlyDictionary<int, CurrencyRateDto> existingCurrencies)
    : ICurrencyWriteRepository
{
    public List<CurrencyRateDto> AddedCurrencies { get; } = [];

    public IReadOnlyCollection<int> LastRequestedIds { get; private set; } = [];

    public bool SaveChangesAsyncCalled { get; private set; }

    public List<CurrencyRateDto> UpdatedCurrencies { get; } = [];

    public Task<IReadOnlyDictionary<int, CurrencyRateDto>> GetByIdsAsync(IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        LastRequestedIds = currencyIds.ToArray();
        return Task.FromResult(existingCurrencies);
    }

    public void AddRange(IEnumerable<CurrencyRateDto> currencyRates)
    {
        AddedCurrencies.AddRange(currencyRates);
    }

    public void UpdateRange(IEnumerable<CurrencyRateDto> currencyRates)
    {
        UpdatedCurrencies.AddRange(currencyRates);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesAsyncCalled = true;
        return Task.CompletedTask;
    }
}
