using Microsoft.EntityFrameworkCore;
using UserService.Abstractions.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class RevokedTokenRepository(UserDbContext dbContext) : IRevokedTokenRepository
{
    public Task<bool> ExistsAsync(string jti, CancellationToken cancellationToken = default)
    {
        return dbContext.RevokedTokens.AnyAsync(revokedToken => revokedToken.Jti == jti, cancellationToken);
    }

    public async Task AddAsync(RevokedToken revokedToken, CancellationToken cancellationToken = default)
    {
        await dbContext.RevokedTokens.AddAsync(revokedToken, cancellationToken);
    }
}
