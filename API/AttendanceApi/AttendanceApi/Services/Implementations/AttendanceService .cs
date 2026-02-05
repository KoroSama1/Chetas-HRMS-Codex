using System.Net;
using AttendanceApi.DTOs.Attendance;
using AttendanceApi.DTOs.HRDto;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;
using SixLabors.ImageSharp;


namespace AttendanceApi.Services.Implementations
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly IFileStorageService _fileStorage;
        private readonly IConfiguration _config;

        public AttendanceService(
            IAttendanceRepository attendanceRepo,
            IFileStorageService fileStorage,
            IConfiguration config
        )
        {
            _attendanceRepo = attendanceRepo;
            _fileStorage = fileStorage;
            _config = config;
        }

        public async Task CheckInAsync(int employeeId, CheckInRequestDto request)
        {
            var now = IstTimeProvider.Now;

            if (
                await _attendanceRepo.HasCheckedInTodayAsync(employeeId, DateOnly.FromDateTime(now))
            )
                throw new AppException(
                    "Employee has already checked in for today.",
                    HttpStatusCode.Conflict
                );

            if (string.IsNullOrWhiteSpace(request.PhotoBase64) || 
                    !request.PhotoBase64.StartsWith("data:image"))
            {
                throw new AppException(
                    "Attendance photo is required for check-in.",
                    HttpStatusCode.BadRequest
                );
            }

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(request.PhotoBase64.Split(',')[1]);

            }
            catch
            {
                throw new AppException(
                    "Invalid attendance photo encoding.",
                    HttpStatusCode.BadRequest
                );
            }

            // 🔒 SIZE CHECK (prevents blank / fake images)
            if (imageBytes.Length < 20_000)
            {
                throw new AppException(
                    "Invalid attendance photo.",
                    HttpStatusCode.BadRequest
                );
            }

            // 🔒 IMAGE VALIDATION (requires ImageSharp)
            using var ms = new MemoryStream(imageBytes);
            using var image = SixLabors.ImageSharp.Image.Load(ms);

            if (image.Width < 200 || image.Height < 200)
            {
                throw new AppException(
                    "Invalid attendance photo.",
                    HttpStatusCode.BadRequest
                );
            }

            var photoPath = await _fileStorage.SaveAttendancePhotoAsync(
                employeeId,
                now,
                imageBytes
            );

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                Date = DateOnly.FromDateTime(now),
                CheckInTime = TimeOnly.FromDateTime(now),
                CheckInStatus = "Pending",
                CheckInLatitude = request.Latitude,
                CheckInLongitude = request.Longitude,
                CheckInLocationAddress = request.LocationAddress,
                CheckInPhotoPath = photoPath,
            };

            await _attendanceRepo.AddAsync(attendance);
        }

        public async Task<List<HrAttendanceDto>> GetTodayByRegionAsync(int regionId, string baseUrl)
        {
            var today = DateOnly.FromDateTime(IstTimeProvider.Now);
            var list = await _attendanceRepo.GetTodayByRegionAsync(regionId, today);

            return list;
        }

        public async Task VerifyAttendanceAsync(
            int attendanceId,
            string status,
            int hrId,
            string type
        )
        {
            if (status != "Confirmed" && status != "Rejected")
                throw new AppException("Invalid status value", HttpStatusCode.Conflict);

            await _attendanceRepo.UpdateStatusAsync(attendanceId, status, hrId, type);
        }

        public async Task CheckOutAsync(int employeeId, CheckOutRequestDto request)
        {
            var now = IstTimeProvider.Now;
            var today = DateOnly.FromDateTime(now);

            var attendance = await _attendanceRepo.GetTodayAttendanceAsync(employeeId, today);

            if (attendance == null)
                throw new AppException(
                    "Employee has not checked in today.",
                    HttpStatusCode.Conflict
                );

            if (attendance.CheckOutTime != null)
                throw new AppException(
                    "Employee has already checked out.",
                    HttpStatusCode.Conflict
                );

            if (string.IsNullOrWhiteSpace(request.PhotoBase64) ||
                !request.PhotoBase64.StartsWith("data:image"))
            {
                throw new AppException(
                    "Attendance photo is required for check-out.",
                    HttpStatusCode.BadRequest
                );
            }

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(request.PhotoBase64.Split(',')[1]);
            }
            catch
            {
                throw new AppException(
                    "Invalid attendance photo encoding.",
                    HttpStatusCode.BadRequest
                );
            }

            if (imageBytes.Length < 20_000)
            {
                throw new AppException(
                    "Invalid attendance photo.",
                    HttpStatusCode.BadRequest
                );
            }

            using var ms = new MemoryStream(imageBytes);
            using var image = SixLabors.ImageSharp.Image.Load(ms);

            if (image.Width < 200 || image.Height < 200)
            {
                throw new AppException(
                    "Invalid attendance photo.",
                    HttpStatusCode.BadRequest
                );
            }


            var photoPath = await _fileStorage.SaveAttendancePhotoAsync(
                employeeId,
                now,
                imageBytes
            );
            attendance.CheckOutStatus = "Pending";
            attendance.CheckOutTime = TimeOnly.FromDateTime(now);
            attendance.CheckOutLatitude = request.Latitude;
            attendance.CheckOutLongitude = request.Longitude;
            attendance.CheckOutLocationAddress = request.LocationAddress;
            attendance.CheckOutPhotoPath = photoPath;

            // 👇 IMPORTANT: Save directly (as you requested)
            await _attendanceRepo.CommitAsync();
        }

        public async Task<List<HrAttendanceDto>> GetTodayPendingCheckInAsync(
            int regionId,
            string baseUrl
        )
        {
            var today = DateOnly.FromDateTime(IstTimeProvider.Now);
            var list = await _attendanceRepo.GetTodayPendingCheckInAsync(regionId, today);

            list.ForEach(x => x.CheckInPhotoUrl = $"{baseUrl}/uploads{x.CheckInPhotoUrl}");

            return list;
        }

        public async Task<List<HrAttendanceDto>> GetTodayPendingCheckOutAsync(
            int regionId,
            string baseUrl
        )
        {
            var today = DateOnly.FromDateTime(IstTimeProvider.Now);
            var list = await _attendanceRepo.GetTodayPendingCheckOutAsync(regionId, today);

            list.ForEach(x => x.CheckOutPhotoUrl = $"{baseUrl}/uploads{x.CheckOutPhotoUrl}");

            return list;
        }

        public async Task<MonthlyAttendanceSummaryDto> GetCurrentMonthSummaryAsync(int employeeId)
        {
            var today = DateTime.UtcNow;
            int year = today.Year;
            int month = today.Month;

            int presentDays = await _attendanceRepo.GetPresentDaysForMonthAsync(
                employeeId,
                year,
                month
            );

            int totalDaysSoFar = today.Day; // current month till today
            int absentDays = totalDaysSoFar - presentDays;

            if (absentDays < 0)
                absentDays = 0;

            return new MonthlyAttendanceSummaryDto
            {
                PresentDays = presentDays,
                AbsentDays = absentDays,
                TotalDays = totalDaysSoFar,
            };
        }
    }
}
