using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class FavoriteCurrencyConfiguration : IEntityTypeConfiguration<FavoriteCurrency>
{
    public void Configure(EntityTypeBuilder<FavoriteCurrency> builder)
    {
        builder.ToTable("favorite");

        builder.HasKey(favoriteCurrency => new
        {
            favoriteCurrency.UserId,
            favoriteCurrency.CurrencyId
        });

        builder.Property(favoriteCurrency => favoriteCurrency.UserId)
            .HasColumnName("user_id");

        builder.Property(favoriteCurrency => favoriteCurrency.CurrencyId)
            .HasColumnName("currency_id");
    }
}
