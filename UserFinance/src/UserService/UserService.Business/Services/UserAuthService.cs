using UserService.Abstractions.Repositories;
using UserService.Abstractions.Security;
using UserService.Abstractions.Models;
using UserService.Abstractions.Services;
using UserService.Business.Metrics;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace UserService.Business.Services;

public sealed class UserAuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator, IRevokedTokenRepository revokedTokenRepository,
    ILogger<UserAuthService> logger) : IUserAuthService
{
    public async Task<User> RegisterAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        UserMetrics.RegisterAttempts.Inc();
        logger.LogInformation("Registering user with name {UserName}.", name);

        if (await userRepository.ExistsByNameAsync(name, cancellationToken))
        {
            UserMetrics.RegisterFailures.Inc();
            logger.LogWarning("Registration failed because user {UserName} already exists.", name);
            throw new UserAlreadyExistsException(name);
        }

        var user = new User(name, passwordHasher.Hash(password));

        await userRepository.AddAsync(user, cancellationToken);

        UserMetrics.RegisteredUsers.Inc();
        return user;
    }

    public async Task<AuthenticationResult> LoginAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        using var loginTimer = UserMetrics.LoginDuration.NewTimer();
        UserMetrics.LoginAttempts.Inc();
        logger.LogInformation("Logging in user with name {UserName}.", name);

        var user = await userRepository.GetByNameAsync(name, cancellationToken);
        if (user is null)
        {
            UserMetrics.LoginFailures.Inc();
            throw new InvalidCredentialsException();
        }

        if (!passwordHasher.Verify(password, user.Password))
        {
            UserMetrics.LoginFailures.Inc();
            logger.LogWarning("Login failed for user {UserName} because credentials are invalid.", name);
            throw new InvalidCredentialsException();
        }

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Name);
        UserMetrics.SuccessfulLogins.Inc();
        logger.LogInformation("User {UserName} logged in successfully.", user.Name);
        return new AuthenticationResult(accessToken);
    }

    public async Task LogoutAsync(string? jwtId, DateTimeOffset? expiresAtUtc,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(jwtId) || expiresAtUtc is null)
        {
            throw new InvalidTokenException();
        }

        logger.LogInformation("Revoking token with jti {JwtId}.", jwtId);

        var revokedToken = new RevokedToken(jwtId, expiresAtUtc.Value.UtcDateTime);
        await revokedTokenRepository.AddAsync(revokedToken, cancellationToken);
        UserMetrics.Logouts.Inc();
    }
}
