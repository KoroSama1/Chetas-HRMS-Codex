using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AttendanceApi.Data.Configurations
{
    public class AttendanceConfig : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> b)
        {
            b.HasKey(x => x.AttendanceId);

            // ---------- DATE ----------
            b.Property(x => x.Date).HasColumnType("date").IsRequired();

            // ---------- CHECK-IN ----------
            b.Property(x => x.CheckInTime).HasColumnType("time").IsRequired();

            b.Property(x => x.CheckInPhotoPath).HasMaxLength(500).IsRequired();

            b.Property(x => x.CheckInLocationAddress).HasMaxLength(300).IsRequired();

            b.Property(x => x.CheckInStatus).HasMaxLength(50).IsRequired();

            // ---------- CHECK-OUT ----------
            b.Property(x => x.CheckOutTime).HasColumnType("time");

            b.Property(x => x.CheckOutPhotoPath).HasMaxLength(500);

            b.Property(x => x.CheckOutLocationAddress).HasMaxLength(300);

            b.Property(x => x.CheckOutStatus).HasMaxLength(50);

            // ---------- HR ----------
            b.Property(x => x.RejectionReason).HasMaxLength(300);

            // ---------- INDEXES ----------
            b.HasIndex(x => new { x.EmployeeId, x.Date }).IsUnique();
            b.HasIndex(x => x.Date);
            b.HasIndex(x => x.CheckInStatus);
            b.HasIndex(x => x.CheckOutStatus);

            // ---------- RELATION ----------
            b.HasOne(x => x.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
