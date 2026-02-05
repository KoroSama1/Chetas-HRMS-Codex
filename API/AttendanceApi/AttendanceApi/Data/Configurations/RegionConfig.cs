using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class RegionConfig : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> b)
        {
            b.HasKey(x => x.RegionId);
            b.Property(x => x.RegionName).IsRequired().HasMaxLength(100);
        }
    }
}
