using UserService.Abstractions.Persistence;

namespace UserService.Tests.Fakes;

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public bool SaveChangesAsyncCalled { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesAsyncCalled = true;
        return Task.FromResult(1);
    }
}
