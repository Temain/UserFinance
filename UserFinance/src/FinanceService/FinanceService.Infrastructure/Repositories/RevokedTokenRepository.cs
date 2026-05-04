using FinanceService.Abstractions.Repositories;
using FinanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure.Repositories;

public sealed class RevokedTokenRepository(FinanceDbContext dbContext) : IRevokedTokenRepository
{
    public Task<bool> ExistsAsync(string jti, CancellationToken cancellationToken = default)
    {
        return dbContext.RevokedTokens.AnyAsync(revokedToken => revokedToken.Jti == jti, cancellationToken);
    }
}
