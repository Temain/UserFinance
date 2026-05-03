using FinanceService.Abstractions.Integrations;
using FinanceService.Abstractions.Repositories;
using FinanceService.Abstractions.Services;
using FinanceService.Domain.Entities;

namespace FinanceService.Business.Services;

public sealed class FinanceService(ICurrencyRepository currencyRepository,
    IUserFavoritesClient userFavoritesClient) : IFinanceService
{
    public async Task<IReadOnlyCollection<Currency>> GetUserFavoriteCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        var favoriteCurrencyIds = await userFavoritesClient.GetUserFavoriteCurrencyIdsAsync(userId, cancellationToken);
        return await currencyRepository.GetByIdsAsync(favoriteCurrencyIds, cancellationToken);
    }

    public async Task<Currency?> GetUserFavoriteCurrencyAsync(long userId, int currencyId,
        CancellationToken cancellationToken = default)
    {
        var favoriteCurrencyIds = await userFavoritesClient.GetUserFavoriteCurrencyIdsAsync(userId, cancellationToken);
        if (!favoriteCurrencyIds.Contains(currencyId))
        {
            return null;
        }

        return await currencyRepository.GetByIdAsync(currencyId, cancellationToken);
    }
}
