namespace AttendanceApi.DTOs.Attendance
{
    public class CheckInRequestDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationAddress { get; set; }
        public string PhotoBase64 { get; set; }
    }
}
