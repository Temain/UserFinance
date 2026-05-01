using UserService.Domain.Entities;

namespace UserService.Abstractions.Repositories;

public interface IUserCurrencyRepository
{
    Task<List<UserCurrency>> GetByUserIdAsync(long userId, CancellationToken cancellationToken = default);

    Task<UserCurrency?> GetByIdAsync(long userId, int currencyId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(long userId, int currencyId, CancellationToken cancellationToken = default);

    Task AddAsync(UserCurrency userCurrency, CancellationToken cancellationToken = default);

    void Remove(UserCurrency userCurrency);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
