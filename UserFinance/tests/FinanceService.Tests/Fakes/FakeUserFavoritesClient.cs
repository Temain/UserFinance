using FinanceService.Abstractions.Integrations;

namespace FinanceService.Tests.Fakes;

internal sealed class FakeUserFavoritesClient(IReadOnlyCollection<int> currencyIds) : IUserFavoritesClient
{
    public Task<IReadOnlyCollection<int>> GetUserFavoriteCurrencyIdsAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(currencyIds);
    }
}
