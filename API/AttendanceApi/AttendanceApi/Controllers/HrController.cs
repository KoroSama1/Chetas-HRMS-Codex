using System.Security.Claims;
using AttendanceApi.DTOs.HRDto;
using AttendanceApi.Services.Implementations;
using AttendanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Controllers
{
    [ApiController]
    [Route("api/hr")]
    [Authorize(Roles = "HR")]
    public class HrController : ControllerBase
    {
        private readonly IRegionService _regionService;
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public HrController(
            IRegionService regionService,
            IAttendanceService attendanceService,
            IEmployeeService employeeService
        )
        {
            _regionService = regionService;
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }

        [HttpGet("regions")]
        public async Task<IActionResult> GetRegions()
        {
            return Ok(await _regionService.GetAllRegionAsync());
        }

        [HttpGet("attendance/today")]
        public async Task<IActionResult> GetTodayAttendance([FromQuery] int regionId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var data = await _attendanceService.GetTodayByRegionAsync(regionId, baseUrl);
            return Ok(data);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyAttendance(
            [FromBody] HrAttendanceDecisionDto request
        )
        {
            var hrId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            await _attendanceService.VerifyAttendanceAsync(
                request.AttendanceId,
                request.Status,
                hrId,
                request.Type
            );

            return Ok(new { message = "Attendance updated successfully" });
        }

        [HttpGet("pending-checkin")]
        public async Task<IActionResult> GetPendingCheckIn([FromQuery] int regionId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var data = await _attendanceService.GetTodayPendingCheckInAsync(regionId, baseUrl);

            return Ok(data);
        }

        [HttpGet("pending-checkout")]
        public async Task<IActionResult> GetPendingCheckOut([FromQuery] int regionId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var data = await _attendanceService.GetTodayPendingCheckOutAsync(regionId, baseUrl);

            return Ok(data);
        }

        [HttpPost("employee")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequestDto request)
        {
            await _employeeService.CreateEmployeeAsync(request);
            return Ok(new { message = "Employee created successfully" });
        }

        [HttpGet("all-employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var employees = await _employeeService.GetAllProfilesAsync(baseUrl);
            return Ok(employees);
        }

        [HttpPut("employee/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(
            int employeeId,
            [FromBody] UpdateEmployeeRequestDto request
        )
        {
            await _employeeService.UpdateEmployeeAsync(employeeId, request);
            return Ok(new { message = "Employee updated successfully" });
        }

        [HttpDelete("employee/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            await _employeeService.DeleteEmployeeAsync(employeeId);
            return Ok(new { message = "Employee deleted successfully" });
        }

        [HttpGet("validate/username")]
        public async Task<IActionResult> ValidateUsername([FromQuery] string value)
        {
            return Ok(await _employeeService.ValidateUsernameAsync(value));
        }

        [HttpGet("validate/email")]
        public async Task<IActionResult> ValidateEmail([FromQuery] string value)
        {
            return Ok(await _employeeService.ValidateEmailAsync(value));
        }

        [HttpGet("validate/phone")]
        public async Task<IActionResult> ValidatePhone([FromQuery] string value)
        {
            return Ok(await _employeeService.ValidatePhoneAsync(value));
        }

        [HttpGet("validate/employee-code")]
        public async Task<IActionResult> ValidateEmployeeCode([FromQuery] int value)
        {
            return Ok(await _employeeService.ValidateEmployeeCodeAsync(value));
        }
    }
}
