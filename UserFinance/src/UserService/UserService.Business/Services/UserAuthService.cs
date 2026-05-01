using UserService.Abstractions.Repositories;
using UserService.Abstractions.Security;
using UserService.Abstractions.Models;
using UserService.Abstractions.Services;
using UserService.Domain.Entities;

namespace UserService.Business.Services;

public sealed class UserAuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IUserAuthService
{
    public async Task<AuthenticationResult> RegisterAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByNameAsync(name, cancellationToken))
        {
            throw new InvalidOperationException($"User with name '{name}' already exists.");
        }

        var user = new User(name, passwordHasher.Hash(password));

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Name);
        return new AuthenticationResult(user, accessToken);
    }

    public async Task<AuthenticationResult> LoginAsync(string name, string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByNameAsync(name, cancellationToken)
            ?? throw new InvalidOperationException("Invalid user credentials.");

        if (!passwordHasher.Verify(password, user.Password))
        {
            throw new InvalidOperationException("Invalid user credentials.");
        }

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Name);
        return new AuthenticationResult(user, accessToken);
    }
}
