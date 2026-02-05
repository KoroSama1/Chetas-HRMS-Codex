using AttendanceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Region> Regions => Set<Region>();
        public DbSet<Designation> Designations => Set<Designation>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
