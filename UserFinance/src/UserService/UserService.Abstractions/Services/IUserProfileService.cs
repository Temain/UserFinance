using UserService.Domain.Entities;

namespace UserService.Abstractions.Services;

public interface IUserProfileService
{
    Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserCurrency>> GetCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default);

    Task AddCurrenciesAsync(long userId, IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default);

    Task RemoveCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default);
}
