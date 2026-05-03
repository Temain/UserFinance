using FinanceService.Abstractions.Repositories;
using FinanceService.Domain.Entities;

namespace FinanceService.Tests.Fakes;

internal sealed class FakeCurrencyRepository : ICurrencyRepository
{
    public Currency? CurrencyById { get; init; }

    public List<Currency> CurrenciesByIds { get; init; } = [];

    public bool GetByIdAsyncCalled { get; private set; }

    public IReadOnlyCollection<int> LastRequestedIds { get; private set; } = [];

    public Task<Currency?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default)
    {
        GetByIdAsyncCalled = true;
        return Task.FromResult(CurrencyById);
    }

    public Task<List<Currency>> GetByIdsAsync(IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        LastRequestedIds = currencyIds.ToArray();
        return Task.FromResult(CurrenciesByIds);
    }

    public Task<List<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new List<Currency>());
    }
}
