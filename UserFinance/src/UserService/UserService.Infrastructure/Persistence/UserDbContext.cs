using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence;

public sealed class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<FavoriteCurrency> FavoriteCurrencies => Set<FavoriteCurrency>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
    }
}
