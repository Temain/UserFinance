using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Persistence.Configurations;

public sealed class RevokedTokenConfiguration : IEntityTypeConfiguration<RevokedToken>
{
    public void Configure(EntityTypeBuilder<RevokedToken> builder)
    {
        builder.ToTable("revoked_token");

        builder.HasKey(revokedToken => revokedToken.Jti);

        builder.Property(revokedToken => revokedToken.Jti)
            .HasColumnName("jti")
            .HasMaxLength(200);

        builder.Property(revokedToken => revokedToken.ExpiresAtUtc)
            .HasColumnName("expires_at_utc");
    }
}
