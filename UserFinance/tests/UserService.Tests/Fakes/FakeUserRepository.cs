using UserService.Abstractions.Repositories;
using UserService.Domain.Entities;

namespace UserService.Tests.Fakes;

internal sealed class FakeUserRepository : IUserRepository
{
    public User? AddedUser { get; private set; }

    public bool AddAsyncCalled { get; private set; }

    public bool ExistsByNameResult { get; init; }

    public bool SaveChangesAsyncCalled { get; private set; }

    public User? UserById { get; init; }

    public User? UserByName { get; init; }

    public Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(UserById);
    }

    public Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(UserByName);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(ExistsByNameResult);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        AddAsyncCalled = true;
        AddedUser = user;

        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesAsyncCalled = true;
        return Task.FromResult(1);
    }
}
