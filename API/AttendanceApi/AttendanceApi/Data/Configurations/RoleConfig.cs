using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> b)
        {
            b.HasKey(x => x.RoleId);
            b.Property(x => x.RoleName).IsRequired().HasMaxLength(50);
        }
    }
}
