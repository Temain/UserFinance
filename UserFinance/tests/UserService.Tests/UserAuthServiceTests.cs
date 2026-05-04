using Microsoft.Extensions.Logging.Abstractions;
using UserService.Abstractions.Models;
using UserService.Business.Services;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;
using UserService.Tests.Fakes;
using Xunit;

namespace UserService.Tests;

public sealed class UserAuthServiceTests
{
    [Fact]
    public async Task RegisterAsync_WhenUserAlreadyExists_ThrowsUserAlreadyExistsException()
    {
        var userRepository = new FakeUserRepository
        {
            ExistsByNameResult = true
        };
        var passwordHasher = new FakePasswordHasher();
        var jwtTokenGenerator = new FakeJwtTokenGenerator();
        var service = new UserAuthService(userRepository, passwordHasher, jwtTokenGenerator,
            NullLogger<UserAuthService>.Instance);

        await Assert.ThrowsAsync<UserAlreadyExistsException>(() => service.RegisterAsync("demo", "secret123"));
        Assert.False(userRepository.AddAsyncCalled);
        Assert.False(userRepository.SaveChangesAsyncCalled);
    }

    [Fact]
    public async Task RegisterAsync_WhenUserIsNew_AddsUserAndReturnsAccessToken()
    {
        var userRepository = new FakeUserRepository();
        var passwordHasher = new FakePasswordHasher();
        var jwtTokenGenerator = new FakeJwtTokenGenerator();
        var service = new UserAuthService(userRepository, passwordHasher, jwtTokenGenerator,
            NullLogger<UserAuthService>.Instance);

        var result = await service.RegisterAsync("demo", "secret123");

        Assert.Equal(new AuthenticationResult("jwt-token"), result);
        Assert.True(userRepository.AddAsyncCalled);
        Assert.True(userRepository.SaveChangesAsyncCalled);
        Assert.NotNull(userRepository.AddedUser);
        Assert.Equal("hashed-secret123", userRepository.AddedUser!.Password);
        Assert.Equal("demo", userRepository.AddedUser.Name);
        Assert.Equal("demo", jwtTokenGenerator.LastUserName);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsInvalid_ThrowsInvalidCredentialsException()
    {
        var userRepository = new FakeUserRepository
        {
            UserByName = new User("demo", "hashed-secret123")
        };
        var passwordHasher = new FakePasswordHasher
        {
            VerifyResult = false
        };
        var jwtTokenGenerator = new FakeJwtTokenGenerator();
        var service = new UserAuthService(userRepository, passwordHasher, jwtTokenGenerator,
            NullLogger<UserAuthService>.Instance);

        await Assert.ThrowsAsync<InvalidCredentialsException>(() => service.LoginAsync("demo", "wrong-password"));
    }
}
