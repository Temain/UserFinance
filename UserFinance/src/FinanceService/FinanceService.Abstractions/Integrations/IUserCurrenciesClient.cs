namespace FinanceService.Abstractions.Integrations;

public interface IUserCurrenciesClient
{
    Task<IReadOnlyCollection<int>> GetUserCurrencyIdsAsync(long userId, CancellationToken cancellationToken = default);
}
