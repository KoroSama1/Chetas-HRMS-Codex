namespace AttendanceApi.DTOs.HRDto
{
    public class CreateEmployeeRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailId { get; set; }

        public DateTime DateOfJoining { get; set; }

        public int RegionId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int RoleId { get; set; }

        // base64 image
        public string? PhotoBase64 { get; set; }
    }
}
