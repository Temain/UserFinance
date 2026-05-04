using UserService.Abstractions.Persistence;

namespace UserService.Infrastructure.Persistence;

public sealed class UnitOfWork(UserDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
