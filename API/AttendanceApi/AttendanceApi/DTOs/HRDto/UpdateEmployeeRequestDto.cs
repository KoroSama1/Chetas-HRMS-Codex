using System.ComponentModel.DataAnnotations;

namespace AttendanceApi.DTOs.HRDto
{
    public class UpdateEmployeeRequestDto
    {
        [Required, MaxLength(150)]
        public string FullName { get; set; }

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required, MaxLength(100)]
        public string EmailId { get; set; }

        public int RegionId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int RoleId { get; set; }
    }
}
