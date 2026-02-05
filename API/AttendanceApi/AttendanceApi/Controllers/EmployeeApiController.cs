using System.Net;
using System.Security.Claims;
using AttendanceApi.DTOs.Attendance;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceApi.Controllers
{
    [ApiController]
    [Route("api/employee")]
    [Authorize]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public EmployeeApiController(
            IAttendanceService attendanceService,
            IEmployeeService employeeService
        )
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }

        // ---------- CHECK-IN ----------
        [HttpPost("check-in")]
        public async Task<IActionResult> CheckInuser([FromBody] CheckInRequestDto request)
        {
            var employeeId = GetEmployeeIduser();
            await _attendanceService.CheckInAsync(employeeId, request);

            return Ok(new { message = "Attendance marked successfully" });
        }

        // ---------- CHECK-OUT ----------
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOutUser([FromBody] CheckOutRequestDto request)
        {
            var employeeId = GetEmployeeIduser();
            await _attendanceService.CheckOutAsync(employeeId, request);

            return Ok(new { message = "Checked out successfully" });
        }

        // ---------- PROFILE ----------
        [HttpGet("profile")]
        public async Task<IActionResult> Profileuser()
        {
            var employeeId = GetEmployeeIduser();
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var profile = await _employeeService.GetMyProfileAsync(employeeId, baseUrl);
            return Ok(profile);
        }

        private int GetEmployeeIduser()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(claim) || !int.TryParse(claim, out var id))
            {
                throw new AppException("Invalid employee identity", HttpStatusCode.Unauthorized);
            }

            return id;
        }

        [HttpGet("attendance/summary/current-month")]
        public async Task<IActionResult> GetCurrentMonthAttendanceSummary()
        {
            var employeeId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var summary = await _attendanceService.GetCurrentMonthSummaryAsync(employeeId);

            return Ok(summary);
        }
    }
}
