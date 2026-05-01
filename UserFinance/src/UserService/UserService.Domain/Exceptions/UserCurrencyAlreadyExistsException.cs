namespace UserService.Domain.Exceptions;

public sealed class UserCurrencyAlreadyExistsException : InvalidOperationException
{
    public UserCurrencyAlreadyExistsException(long userId, int currencyId)
        : base($"Currency '{currencyId}' is already assigned to user '{userId}'.")
    {
    }
}
