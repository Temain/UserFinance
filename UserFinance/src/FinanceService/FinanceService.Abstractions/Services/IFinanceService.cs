using FinanceService.Domain.Entities;

namespace FinanceService.Abstractions.Services;

public interface IFinanceService
{
    Task<IReadOnlyCollection<Currency>> GetUserFavoriteCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default);

    Task<Currency?> GetUserFavoriteCurrencyAsync(long userId, int currencyId,
        CancellationToken cancellationToken = default);
}
