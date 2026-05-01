namespace FinanceService.Domain.Entities;

public sealed class Currency
{
    private Currency()
    {
    }

    public Currency(int id, string name, decimal rate)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "Currency id must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Currency name is required.", nameof(name));
        }

        if (rate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "Currency rate must be greater than zero.");
        }

        Id = id;
        Name = name.Trim();
        Rate = rate;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public decimal Rate { get; private set; }

    public void UpdateRate(decimal rate)
    {
        if (rate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "Currency rate must be greater than zero.");
        }

        Rate = rate;
    }
}
