using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Application.Models;
using Microsoft.EntityFrameworkCore;
using CurrencyUpdater.Infrastructure.Persistence;

namespace CurrencyUpdater.Infrastructure.Repositories;

public sealed class CurrencyWriteRepository(CurrencyUpdateDbContext dbContext) : ICurrencyWriteRepository
{
    public async Task UpsertAsync(IReadOnlyCollection<CurrencyRateDto> currencyRates,
        CancellationToken cancellationToken = default)
    {
        if (currencyRates.Count == 0)
        {
            return;
        }

        var currencyIds = currencyRates.Select(currency => currency.CurrencyId).ToArray();
        var existingCurrencies = await dbContext.Currencies
            .Where(currency => currencyIds.Contains(currency.Id))
            .ToDictionaryAsync(currency => currency.Id, cancellationToken);

        foreach (var currencyRate in currencyRates)
        {
            if (existingCurrencies.TryGetValue(currencyRate.CurrencyId, out var existingCurrency))
            {
                existingCurrency.Update(currencyRate.CurrencyName, currencyRate.Rate);
                continue;
            }

            dbContext.Currencies.Add(
                new CurrencyRecord(currencyRate.CurrencyId, currencyRate.CurrencyName, currencyRate.Rate));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
