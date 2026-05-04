using FinanceService.Abstractions.Integrations;
using FinanceService.Abstractions.Repositories;
using FinanceService.Abstractions.Services;
using FinanceService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FinanceService.Business.Services;

public sealed class FinanceService(ICurrencyRepository currencyRepository,
    IUserFavoritesClient userFavoritesClient, ILogger<FinanceService> logger) : IFinanceService
{
    public async Task<IReadOnlyCollection<Currency>> GetUserFavoriteCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching rates for favorite currencies of user {UserId}.", userId);
        var favoriteCurrencyIds = await userFavoritesClient.GetUserFavoriteCurrencyIdsAsync(userId, cancellationToken);
        return await currencyRepository.GetByIdsAsync(favoriteCurrencyIds, cancellationToken);
    }

    public async Task<Currency?> GetUserFavoriteCurrencyAsync(long userId, int currencyId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching rate for favorite currency {CurrencyId} of user {UserId}.", currencyId, userId);
        var favoriteCurrencyIds = await userFavoritesClient.GetUserFavoriteCurrencyIdsAsync(userId, cancellationToken);
        if (!favoriteCurrencyIds.Contains(currencyId))
        {
            logger.LogInformation("Currency {CurrencyId} is not a favorite for user {UserId}.", currencyId, userId);
            return null;
        }

        return await currencyRepository.GetByIdAsync(currencyId, cancellationToken);
    }
}
