using FinanceService.Domain.Entities;

namespace FinanceService.Abstractions.Repositories;

public interface ICurrencyRepository
{
    Task<Currency?> GetByIdAsync(int currencyId, CancellationToken cancellationToken = default);

    Task<List<Currency>> GetByIdsAsync(IReadOnlyCollection<int> currencyIds,
        CancellationToken cancellationToken = default);

    Task<List<Currency>> GetAllAsync(CancellationToken cancellationToken = default);
}
