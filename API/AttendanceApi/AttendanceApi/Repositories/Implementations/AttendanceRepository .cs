using AttendanceApi.Data;
using AttendanceApi.DTOs.HRDto;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasCheckedInTodayAsync(int employeeId, DateOnly date)
        {
            return await _context.Attendances.AnyAsync(a =>
                a.EmployeeId == employeeId && a.Date == date
            );
        }

        public async Task AddAsync(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<HrAttendanceDto>> GetTodayByRegionAsync(int regionId, DateOnly date)
        {
            return await _context
                .Attendances.Where(a => a.Date == date && a.Employee.RegionId == regionId)
                .Select(a => new HrAttendanceDto
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeName = a.Employee.FullName,
                    //Date = a.Date,
                    //CheckInTime = a.CheckInTime,
                    //CheckInLocationAddress = a.CheckInLocationAddress,
                    //CheckInPhotoUrl = a.CheckInPhotoPath   // ONLY RELATIVE PATH
                })
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int attendanceId, string status, int hrId, string type)
        {
            var attendance = await _context.Attendances.FirstOrDefaultAsync(a =>
                a.AttendanceId == attendanceId
            );

            if (attendance == null)
                throw new Exception("Attendance record not found");

            if (type == "CheckIn")
            {
                attendance.CheckInStatus = status;
            }
            else if (type == "CheckOut")
            {
                attendance.CheckOutStatus = status;
            }

            attendance.VerifiedByHRId = hrId;

            await _context.SaveChangesAsync();
        }

        public async Task<Attendance?> GetTodayAttendanceAsync(int employeeId, DateOnly date) //while checkout needed
        {
            return await _context.Attendances.FirstOrDefaultAsync(a =>
                a.EmployeeId == employeeId && a.Date == date
            );
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<HrAttendanceDto>> GetTodayPendingCheckInAsync(
            int regionId,
            DateOnly date
        )
        {
            return await _context
                .Attendances.Where(a =>
                    a.Date == date
                    && a.Employee.RegionId == regionId
                    && a.CheckInStatus == "Pending"
                //&&
                //a.CheckOutTime == null
                )
                .Select(a => new HrAttendanceDto
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeName = a.Employee.FullName,
                    CheckInTime = a.CheckInTime,
                    CheckInLocationAddress = a.CheckInLocationAddress,
                    CheckInPhotoUrl = a.CheckInPhotoPath,
                })
                .ToListAsync();
        }

        public async Task<List<HrAttendanceDto>> GetTodayPendingCheckOutAsync(
            int regionId,
            DateOnly date
        )
        {
            return await _context
                .Attendances.Where(a =>
                    a.Date == date
                    && a.Employee.RegionId == regionId
                    && a.CheckOutStatus == "Pending"
                    && a.CheckOutTime != null
                )
                .Select(a => new HrAttendanceDto
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeName = a.Employee.FullName,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime,
                    CheckOutLocationAddress = a.CheckOutLocationAddress,
                    CheckOutPhotoUrl = a.CheckOutPhotoPath,
                })
                .ToListAsync();
        }

        public async Task<int> GetPresentDaysForMonthAsync(int employeeId, int year, int month)
        {
            return await _context
                .Attendances.Where(a =>
                    a.EmployeeId == employeeId
                    && a.Date.Year == year
                    && a.Date.Month == month
                    && a.CheckInStatus == "Confirmed"
                    && a.CheckOutStatus == "Confirmed"
                )
                .Select(a => a.Date)
                .Distinct()
                .CountAsync();
        }
    }
}
