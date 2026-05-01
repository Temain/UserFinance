namespace UserService.Domain.Entities;

public sealed class UserCurrency
{
    private UserCurrency()
    {
    }

    public UserCurrency(long userId, int currencyId)
    {
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId), "User id must be greater than zero.");
        }

        if (currencyId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currencyId), "Currency id must be greater than zero.");
        }

        UserId = userId;
        CurrencyId = currencyId;
    }

    public long UserId { get; private set; }

    public int CurrencyId { get; private set; }
}
