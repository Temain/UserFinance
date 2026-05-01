using UserService.Abstractions.Models;

namespace UserService.Abstractions.Services;

public interface IUserAuthService
{
    Task<AuthenticationResult> RegisterAsync(string name, string password,
        CancellationToken cancellationToken = default);

    Task<AuthenticationResult> LoginAsync(string name, string password, CancellationToken cancellationToken = default);
}
