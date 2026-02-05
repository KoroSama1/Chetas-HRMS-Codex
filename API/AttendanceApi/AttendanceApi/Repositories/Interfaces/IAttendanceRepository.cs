using AttendanceApi.DTOs.HRDto;
using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<bool> HasCheckedInTodayAsync(int employeeId, DateOnly date);
        Task AddAsync(Attendance attendance);

        Task<List<HrAttendanceDto>> GetTodayByRegionAsync(int regionId, DateOnly date);
        Task UpdateStatusAsync(int attendanceId, string status, int hrId, string type);

        Task<Attendance?> GetTodayAttendanceAsync(int employeeId, DateOnly date); //while checkout
        Task CommitAsync();

        Task<List<HrAttendanceDto>> GetTodayPendingCheckInAsync(int regionId, DateOnly date);
        Task<List<HrAttendanceDto>> GetTodayPendingCheckOutAsync(int regionId, DateOnly date);

        Task<int> GetPresentDaysForMonthAsync(int employeeId, int year, int month);
    }
}
