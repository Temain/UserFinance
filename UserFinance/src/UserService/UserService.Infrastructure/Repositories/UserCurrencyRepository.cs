using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class UserCurrencyRepository(UserDbContext dbContext)
{
    public Task<List<UserCurrency>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return dbContext.UserCurrencies
            .AsNoTracking()
            .Where(userCurrency => userCurrency.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public Task<UserCurrency?> GetByIdAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        return dbContext.UserCurrencies.FirstOrDefaultAsync(
            userCurrency => userCurrency.UserId == userId && userCurrency.CurrencyId == currencyId,
            cancellationToken);
    }

    public Task<bool> ExistsAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        return dbContext.UserCurrencies.AnyAsync(
            userCurrency => userCurrency.UserId == userId && userCurrency.CurrencyId == currencyId,
            cancellationToken);
    }

    public async Task AddAsync(UserCurrency userCurrency, CancellationToken cancellationToken = default)
    {
        await dbContext.UserCurrencies.AddAsync(userCurrency, cancellationToken);
    }

    public void Remove(UserCurrency userCurrency)
    {
        dbContext.UserCurrencies.Remove(userCurrency);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
