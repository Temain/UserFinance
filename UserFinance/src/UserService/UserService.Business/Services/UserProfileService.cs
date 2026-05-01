using UserService.Abstractions.Repositories;
using UserService.Abstractions.Services;
using UserService.Domain.Entities;

namespace UserService.Business.Services;

public sealed class UserProfileService(IUserRepository userRepository,
    IUserCurrencyRepository userCurrencyRepository) : IUserProfileService
{
    public Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<UserCurrency>> GetCurrenciesAsync(long userId,
        CancellationToken cancellationToken = default)
    {
        return await userCurrencyRepository.GetByUserIdAsync(userId, cancellationToken);
    }

    public async Task AddCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException($"User with id '{userId}' was not found.");

        user.AddCurrency(currencyId);
        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException($"User with id '{userId}' was not found.");

        user.RemoveCurrency(currencyId);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
