using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        //[Required, MaxLength(50)]
        //public string EmployeeCode { get; set; }
        // 🔥 UPDATED: EmployeeCode is now INT
        [Required]
        public int EmployeeCode { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public string EmailId { get; set; }

        public DateTime DateOfJoining { get; set; }

        public string? EmployeePhotoPath { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int DesignationId { get; set; }
        public Designation Designation { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        // 🔥 SOFT DELETE
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
