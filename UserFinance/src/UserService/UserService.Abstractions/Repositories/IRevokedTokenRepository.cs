using UserService.Domain.Entities;

namespace UserService.Abstractions.Repositories;

public interface IRevokedTokenRepository
{
    Task<bool> ExistsAsync(string jti, CancellationToken cancellationToken = default);

    Task AddAsync(RevokedToken revokedToken, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
