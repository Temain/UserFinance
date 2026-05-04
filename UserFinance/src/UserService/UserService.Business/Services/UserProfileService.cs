using UserService.Abstractions.Repositories;
using UserService.Abstractions.Services;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace UserService.Business.Services;

public sealed class UserProfileService(IUserRepository userRepository,
    IFavoriteCurrencyRepository favoriteCurrencyRepository, ILogger<UserProfileService> logger) : IUserProfileService
{
    public Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<FavoriteCurrency>> GetFavoriteCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching favorite currencies for user {UserId}.", userId);
        return await favoriteCurrencyRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task AddFavoriteCurrenciesAsync(long userId, IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Adding {FavoriteCurrencyCount} favorite currencies for user {UserId}.",
            currencyIds.Count, userId);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        foreach (var currencyId in currencyIds)
        {
            user.AddFavoriteCurrency(currencyId);
        }

        await userRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Favorite currencies added for user {UserId}.", userId);
    }

    public async Task RemoveFavoriteCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Removing favorite currency {CurrencyId} for user {UserId}.", currencyId, userId);
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        user.RemoveFavoriteCurrency(currencyId);
        await userRepository.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Favorite currency {CurrencyId} removed for user {UserId}.", currencyId, userId);
    }
}
