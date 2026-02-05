using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> b)
        {
            b.HasKey(x => x.EmployeeId);

            b.HasIndex(x => x.Username).IsUnique().HasFilter("[IsDeleted] = 0");
            b.HasIndex(x => x.EmployeeCode).IsUnique().HasFilter("[IsDeleted] = 0");
            b.HasIndex(x => x.EmailId).IsUnique().HasFilter("[IsDeleted] = 0");
            b.HasIndex(x => x.PhoneNumber).IsUnique().HasFilter("[IsDeleted] = 0");

            b.HasOne(x => x.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Region)
                .WithMany(r => r.Employees)
                .HasForeignKey(x => x.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Designation)
                .WithMany(d => d.Employees)
                .HasForeignKey(x => x.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
