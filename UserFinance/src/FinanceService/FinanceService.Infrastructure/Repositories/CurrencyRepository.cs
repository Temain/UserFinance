using FinanceService.Domain.Entities;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories;

public sealed class CurrencyRepository(FinanceDbContext dbContext)
{
    public Task<Currency?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default)
    {
        return dbContext.Currencies
            .AsNoTracking()
            .FirstOrDefaultAsync(currency => currency.Id == currencyId, cancellationToken);
    }

    public Task<List<Currency>> GetByIdsAsync(
        IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default)
    {
        if (currencyIds.Count == 0)
        {
            return Task.FromResult(new List<Currency>());
        }

        return dbContext.Currencies
            .AsNoTracking()
            .Where(currency => currencyIds.Contains(currency.Id))
            .OrderBy(currency => currency.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Currencies
            .AsNoTracking()
            .OrderBy(currency => currency.Id)
            .ToListAsync(cancellationToken);
    }
}
