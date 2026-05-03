using Microsoft.EntityFrameworkCore;
using UserService.Abstractions.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class FavoriteCurrencyRepository(UserDbContext dbContext) : IFavoriteCurrencyRepository
{
    public Task<List<FavoriteCurrency>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return dbContext.FavoriteCurrencies
            .AsNoTracking()
            .Where(favoriteCurrency => favoriteCurrency.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public Task<FavoriteCurrency?> GetByIdAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        return dbContext.FavoriteCurrencies.FirstOrDefaultAsync(
            favoriteCurrency => favoriteCurrency.UserId == userId && favoriteCurrency.CurrencyId == currencyId,
            cancellationToken);
    }

    public Task<bool> ExistsAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        return dbContext.FavoriteCurrencies.AnyAsync(
            favoriteCurrency => favoriteCurrency.UserId == userId && favoriteCurrency.CurrencyId == currencyId,
            cancellationToken);
    }

    public async Task AddAsync(FavoriteCurrency favoriteCurrency, CancellationToken cancellationToken = default)
    {
        await dbContext.FavoriteCurrencies.AddAsync(favoriteCurrency, cancellationToken);
    }

    public void Remove(FavoriteCurrency favoriteCurrency)
    {
        dbContext.FavoriteCurrencies.Remove(favoriteCurrency);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
