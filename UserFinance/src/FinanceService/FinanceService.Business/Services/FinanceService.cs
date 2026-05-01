using FinanceService.Abstractions.Integrations;
using FinanceService.Abstractions.Repositories;
using FinanceService.Abstractions.Services;
using FinanceService.Domain.Entities;

namespace FinanceService.Business.Services;

public sealed class FinanceService(ICurrencyRepository currencyRepository,
    IUserCurrenciesClient userCurrenciesClient) : IFinanceService
{
    public async Task<IReadOnlyCollection<Currency>> GetUserCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        var userCurrencyIds = await userCurrenciesClient.GetUserCurrencyIdsAsync(userId, cancellationToken);
        return await currencyRepository.GetByIdsAsync(userCurrencyIds, cancellationToken);
    }

    public async Task<Currency?> GetUserCurrencyAsync(long userId, int currencyId,
        CancellationToken cancellationToken = default)
    {
        var userCurrencyIds = await userCurrenciesClient.GetUserCurrencyIdsAsync(userId, cancellationToken);
        if (!userCurrencyIds.Contains(currencyId))
        {
            return null;
        }

        return await currencyRepository.GetByIdAsync(currencyId, cancellationToken);
    }
}
