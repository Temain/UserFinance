using UserService.Abstractions.Repositories;
using UserService.Abstractions.Services;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

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

    public async Task AddCurrenciesAsync(long userId, IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        foreach (var currencyId in currencyIds)
        {
            user.AddCurrency(currencyId);
        }

        await userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveCurrencyAsync(long userId, int currencyId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        user.RemoveCurrency(currencyId);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}
