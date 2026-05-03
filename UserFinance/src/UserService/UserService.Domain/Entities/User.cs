using UserService.Domain.Exceptions;

namespace UserService.Domain.Entities;

public sealed class User
{
    private readonly List<FavoriteCurrency> _favoriteCurrencies = [];

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

    public IReadOnlyCollection<FavoriteCurrency> FavoriteCurrencies => _favoriteCurrencies.AsReadOnly();

    public void AddFavoriteCurrency(int currencyId)
    {
        if (currencyId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currencyId), "Currency id must be greater than zero.");
        }

        if (_favoriteCurrencies.Any(x => x.CurrencyId == currencyId))
        {
            throw new FavoriteCurrencyAlreadyExistsException(Id, currencyId);
        }

        _favoriteCurrencies.Add(new FavoriteCurrency(Id, currencyId));
    }

    public void RemoveFavoriteCurrency(int currencyId)
    {
        var favoriteCurrency = _favoriteCurrencies.FirstOrDefault(x => x.CurrencyId == currencyId);

        if (favoriteCurrency is null)
        {
            return;
        }

        _favoriteCurrencies.Remove(favoriteCurrency);
    }
}
