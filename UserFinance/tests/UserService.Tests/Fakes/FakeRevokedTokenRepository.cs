using UserService.Abstractions.Repositories;
using UserService.Domain.Entities;

namespace UserService.Tests.Fakes;

internal sealed class FakeRevokedTokenRepository : IRevokedTokenRepository
{
    public readonly List<RevokedToken> RevokedTokens = [];

    public bool SaveChangesAsyncCalled { get; private set; }

    public Task<bool> ExistsAsync(string jti, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(RevokedTokens.Any(revokedToken => revokedToken.Jti == jti));
    }

    public Task AddAsync(RevokedToken revokedToken, CancellationToken cancellationToken = default)
    {
        RevokedTokens.Add(revokedToken);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesAsyncCalled = true;
        return Task.FromResult(1);
    }
}
