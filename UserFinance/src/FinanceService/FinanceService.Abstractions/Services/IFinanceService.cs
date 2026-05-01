using FinanceService.Domain.Entities;

namespace FinanceService.Abstractions.Services;

public interface IFinanceService
{
    Task<IReadOnlyCollection<Currency>> GetUserCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default);

    Task<Currency?> GetUserCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default);
}
