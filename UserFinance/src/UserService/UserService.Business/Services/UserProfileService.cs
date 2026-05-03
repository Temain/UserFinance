using UserService.Abstractions.Repositories;
using UserService.Abstractions.Services;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.Business.Services;

public sealed class UserProfileService(IUserRepository userRepository,
    IFavoriteCurrencyRepository favoriteCurrencyRepository) : IUserProfileService
{
    public Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<FavoriteCurrency>> GetFavoriteCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        return await favoriteCurrencyRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task AddFavoriteCurrenciesAsync(long userId, IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        foreach (var currencyId in currencyIds)
        {
            user.AddFavoriteCurrency(currencyId);
        }

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveFavoriteCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        user.RemoveFavoriteCurrency(currencyId);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
