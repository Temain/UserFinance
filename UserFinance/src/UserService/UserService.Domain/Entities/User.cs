using UserService.Domain.Exceptions;

namespace UserService.Domain.Entities;

public sealed class User
{
    private readonly List<UserCurrency> _currencies = [];

    private User()
    {
    }

    public User(string name, string password)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("User name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password is required.", nameof(password));
        }

        Name = name.Trim();
        Password = password.Trim();
    }

    public long Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public IReadOnlyCollection<UserCurrency> Currencies => _currencies.AsReadOnly();

    public void AddCurrency(int currencyId)
    {
        if (currencyId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currencyId), "Currency id must be greater than zero.");
        }

        if (_currencies.Any(x => x.CurrencyId == currencyId))
        {
            throw new UserCurrencyAlreadyExistsException(Id, currencyId);
        }

        _currencies.Add(new UserCurrency(Id, currencyId));
    }

    public void RemoveCurrency(int currencyId)
    {
        var userCurrency = _currencies.FirstOrDefault(x => x.CurrencyId == currencyId);

        if (userCurrency is null)
        {
            return;
        }

        _currencies.Remove(userCurrency);
    }
}
