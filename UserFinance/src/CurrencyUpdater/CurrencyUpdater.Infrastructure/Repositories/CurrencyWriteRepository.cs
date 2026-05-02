using CurrencyUpdater.Application.Abstractions;
using CurrencyUpdater.Application.Models;
using CurrencyUpdater.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdater.Infrastructure.Repositories;

public sealed class CurrencyWriteRepository(CurrencyUpdateDbContext dbContext) : ICurrencyWriteRepository
{
    public async Task<IReadOnlyDictionary<int, CurrencyRateDto>> GetByIdsAsync(IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        if (currencyIds.Count == 0)
        {
            return new Dictionary<int, CurrencyRateDto>();
        }

        return await dbContext.Currencies
            .AsNoTracking()
            .Where(currency => currencyIds.Contains(currency.Id))
            .ToDictionaryAsync(currency => currency.Id,
                currency => new CurrencyRateDto(currency.Id, currency.Name, currency.Rate), cancellationToken);
    }

    public void AddRange(IEnumerable<CurrencyRateDto> currencyRates)
    {
        var currencies = currencyRates
            .Select(currency => new CurrencyRecord(currency.CurrencyId, currency.CurrencyName, currency.Rate))
            .ToArray();

        dbContext.Currencies.AddRange(currencies);
    }

    public void UpdateRange(IEnumerable<CurrencyRateDto> currencyRates)
    {
        foreach (var currencyRate in currencyRates)
        {
            var currency = new CurrencyRecord(currencyRate.CurrencyId, currencyRate.CurrencyName, currencyRate.Rate);
            dbContext.Currencies.Attach(currency);
            dbContext.Entry(currency).State = EntityState.Modified;
        }
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
