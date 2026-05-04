using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceService.Infrastructure.Persistence.Configurations;

public sealed class RevokedTokenRecordConfiguration : IEntityTypeConfiguration<RevokedTokenRecord>
{
    public void Configure(EntityTypeBuilder<RevokedTokenRecord> builder)
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
