using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> b)
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => x.TokenId).IsUnique();

            b.Property(x => x.TokenHash).IsRequired();

            b.Property(x => x.CreatedAt).HasColumnType("datetime2(7)").IsRequired();

            b.Property(x => x.ExpiresAt).HasColumnType("datetime2(7)").IsRequired();

            b.Property(x => x.IsRevoked).HasDefaultValue(false);
        }
    }
}
