using AttendanceApi.DTOs.Attendance;
using AttendanceApi.DTOs.HRDto;

namespace AttendanceApi.Services.Interfaces
{
    public interface IAttendanceService
    {
        Task CheckInAsync(int employeeId, CheckInRequestDto request);
        Task<List<HrAttendanceDto>> GetTodayByRegionAsync(int regionId, string baseUrl);

        Task VerifyAttendanceAsync(int attendanceId, string status, int hrId, string type);

        Task CheckOutAsync(int employeeId, CheckOutRequestDto request); //for checkout

        Task<List<HrAttendanceDto>> GetTodayPendingCheckInAsync(int regionId, string baseUrl);
        Task<List<HrAttendanceDto>> GetTodayPendingCheckOutAsync(int regionId, string baseUrl);

        Task<MonthlyAttendanceSummaryDto> GetCurrentMonthSummaryAsync(int employeeId);
    }
}
