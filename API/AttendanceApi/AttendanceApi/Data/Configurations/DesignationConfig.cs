using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class DesignationConfig : IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> b)
        {
            b.HasKey(x => x.DesignationId);
            b.Property(x => x.DesignationName).IsRequired().HasMaxLength(100);
        }
    }
}
