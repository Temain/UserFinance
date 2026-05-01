using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id).HasColumnName("id").ValueGeneratedNever();

        builder.Property(user => user.Name).HasColumnName("name").HasMaxLength(200).IsRequired();

        builder.Property(user => user.Password).HasColumnName("password").HasMaxLength(500).IsRequired();

        builder.Navigation(user => user.Currencies).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(user => user.Currencies).WithOne().HasForeignKey(userCurrency => userCurrency.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
