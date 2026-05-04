namespace FinanceService.Infrastructure.Persistence;

public sealed class RevokedTokenRecord
{
    public string Jti { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }
}
