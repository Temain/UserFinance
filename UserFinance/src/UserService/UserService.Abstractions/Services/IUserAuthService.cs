using UserService.Abstractions.Models;
using UserService.Domain.Entities;

namespace UserService.Abstractions.Services;

public interface IUserAuthService
{
    Task<User> RegisterAsync(string name, string password,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResult> LoginAsync(string name, string password, CancellationToken cancellationToken = default);

    Task LogoutAsync(string? jwtId, DateTimeOffset? expiresAtUtc, CancellationToken cancellationToken = default);
}
