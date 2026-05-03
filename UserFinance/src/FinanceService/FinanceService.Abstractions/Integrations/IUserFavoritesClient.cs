namespace FinanceService.Abstractions.Integrations;

public interface IUserFavoritesClient
{
    Task<IReadOnlyCollection<int>> GetUserFavoriteCurrencyIdsAsync(long userId,
        CancellationToken cancellationToken = default);
}
