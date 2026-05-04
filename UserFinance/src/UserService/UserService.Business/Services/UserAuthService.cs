using UserService.Abstractions.Repositories;
using UserService.Abstractions.Security;
using UserService.Abstractions.Models;
using UserService.Abstractions.Services;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace UserService.Business.Services;

public sealed class UserAuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator, IRevokedTokenRepository revokedTokenRepository,
    ILogger<UserAuthService> logger) : IUserAuthService
{
    public async Task<AuthenticationResult> RegisterAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Registering user with name {UserName}.", name);

        if (await userRepository.ExistsByNameAsync(name, cancellationToken))
        {
            logger.LogWarning("Registration failed because user {UserName} already exists.", name);
            throw new UserAlreadyExistsException(name);
        }

        var user = new User(name, passwordHasher.Hash(password));

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Name);
        logger.LogInformation("User {UserName} registered successfully with id {UserId}.", user.Name, user.Id);
        return new AuthenticationResult(accessToken);
    }

    public async Task<AuthenticationResult> LoginAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Logging in user with name {UserName}.", name);

        var user = await userRepository.GetByNameAsync(name, cancellationToken)
            ?? throw new InvalidCredentialsException();

        if (!passwordHasher.Verify(password, user.Password))
        {
            logger.LogWarning("Login failed for user {UserName} because credentials are invalid.", name);
            throw new InvalidCredentialsException();
        }

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Name);
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
        await revokedTokenRepository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Token with jti {JwtId} revoked successfully.", jwtId);
    }
}
