using FinanceService.Abstractions.Integrations;

namespace FinanceService.Tests.Fakes;

internal sealed class FakeUserCurrenciesClient(IReadOnlyCollection<int> currencyIds) : IUserCurrenciesClient
{
    public Task<IReadOnlyCollection<int>> GetUserCurrencyIdsAsync(long userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(currencyIds);
    }
}
