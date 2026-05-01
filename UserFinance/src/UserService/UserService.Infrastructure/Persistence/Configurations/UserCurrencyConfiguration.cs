using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class UserCurrencyConfiguration : IEntityTypeConfiguration<UserCurrency>
{
    public void Configure(EntityTypeBuilder<UserCurrency> builder)
    {
        builder.ToTable("user_currency");

        builder.HasKey(userCurrency => new
        {
            userCurrency.UserId,
            userCurrency.CurrencyId
        });

        builder.Property(userCurrency => userCurrency.UserId)
            .HasColumnName("user_id");

        builder.Property(userCurrency => userCurrency.CurrencyId)
            .HasColumnName("currency_id");
    }
}
