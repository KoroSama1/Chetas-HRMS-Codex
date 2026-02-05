namespace AttendanceApi.DTOs.HRDto
{
    public class HrAttendanceDto
    {
        //public int AttendanceId { get; set; }
        //public string EmployeeName { get; set; }
        //public DateOnly Date { get; set; }
        //public TimeOnly CheckInTime { get; set; }
        //public string CheckInLocationAddress { get; set; }
        //public string CheckInPhotoUrl { get; set; }

        public int AttendanceId { get; set; }
        public string EmployeeName { get; set; }

        public TimeOnly CheckInTime { get; set; }
        public TimeOnly? CheckOutTime { get; set; }

        public string? CheckInLocationAddress { get; set; }
        public string? CheckOutLocationAddress { get; set; }

        public string? CheckInPhotoUrl { get; set; }
        public string? CheckOutPhotoUrl { get; set; }
    }
}
