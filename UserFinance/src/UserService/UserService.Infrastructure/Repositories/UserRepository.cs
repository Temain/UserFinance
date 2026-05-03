using Microsoft.EntityFrameworkCore;
using UserService.Abstractions.Repositories;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class UserRepository(UserDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByIdAsync(long userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.FavoriteCurrencies)
            .FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);
    }

    public Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return dbContext.Users
            .Include(user => user.FavoriteCurrencies)
            .FirstOrDefaultAsync(user => user.Name == name, cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return dbContext.Users.AnyAsync(user => user.Name == name, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
