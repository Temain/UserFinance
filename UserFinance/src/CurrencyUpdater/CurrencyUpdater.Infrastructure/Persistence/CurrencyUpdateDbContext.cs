using Microsoft.EntityFrameworkCore;

namespace CurrencyUpdater.Infrastructure.Persistence;

public sealed class CurrencyUpdateDbContext(DbContextOptions<CurrencyUpdateDbContext> options) : DbContext(options)
{
    public DbSet<CurrencyRecord> Currencies => Set<CurrencyRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CurrencyUpdateDbContext).Assembly);
    }
}
