using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> b)
        {
            b.HasKey(x => x.DepartmentId);
            b.Property(x => x.DepartmentName).IsRequired().HasMaxLength(100);
        }
    }
}
