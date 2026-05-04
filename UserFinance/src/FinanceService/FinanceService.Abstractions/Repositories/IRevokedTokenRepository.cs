namespace FinanceService.Abstractions.Repositories;

public interface IRevokedTokenRepository
{
    Task<bool> ExistsAsync(string jti, CancellationToken cancellationToken = default);
}
