using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public sealed class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("currency");

        builder.HasKey(currency => currency.Id);

        builder.Property(currency => currency.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(currency => currency.Name).HasColumnName("name").HasMaxLength(100).IsRequired();

        builder.Property(currency => currency.Rate).HasColumnName("rate").HasPrecision(18, 6).IsRequired();
    }
}
