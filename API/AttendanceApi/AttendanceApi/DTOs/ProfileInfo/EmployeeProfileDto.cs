namespace AttendanceApi.DTOs.ProfileInfo
{
    public class EmployeeProfileDto
    {
        public int EmployeeId { get; set; }
        public int EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfJoining { get; set; }

        public string Region { get; set; }
        public int RegionId { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public string Designation { get; set; }
        public int DesignationId { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public string EmployeePhotoPath { get; set; }
    }
}
