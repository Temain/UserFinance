using UserService.Domain.Entities;

namespace UserService.Abstractions.Services;

public interface IUserProfileService
{
    Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FavoriteCurrency>> GetFavoriteCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default);

    Task AddFavoriteCurrenciesAsync(long userId, IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default);

    Task RemoveFavoriteCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default);
}
