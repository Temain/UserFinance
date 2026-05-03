namespace UserService.Domain.Exceptions;

public sealed class FavoriteCurrencyAlreadyExistsException : InvalidOperationException
{
    public FavoriteCurrencyAlreadyExistsException(long userId, int currencyId)
        : base($"Favorite currency '{currencyId}' is already assigned to user '{userId}'.")
    {
    }
}
