namespace FinanceService.Abstractions.Integrations;

public interface IRevokedTokenClient
{
    Task<bool> IsRevokedAsync(string jti, CancellationToken cancellationToken = default);
}
