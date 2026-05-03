using UserService.Domain.Entities;

namespace UserService.Abstractions.Repositories;

public interface IFavoriteCurrencyRepository
{
    Task<List<FavoriteCurrency>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);

    Task<FavoriteCurrency?> GetByIdAsync(long userId, int currencyId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long userId, int currencyId, CancellationToken cancellationToken = default);

    Task AddAsync(FavoriteCurrency favoriteCurrency, CancellationToken cancellationToken = default);

    void Remove(FavoriteCurrency favoriteCurrency);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
