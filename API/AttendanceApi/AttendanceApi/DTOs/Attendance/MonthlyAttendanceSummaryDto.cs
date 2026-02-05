namespace AttendanceApi.DTOs.Attendance
{
    public class MonthlyAttendanceSummaryDto
    {
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int TotalDays { get; set; }
    }
}
