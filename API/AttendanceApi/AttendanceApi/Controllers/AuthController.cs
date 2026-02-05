using System.Security.Claims;
using AttendanceApi.DTOs.Auth;
using AttendanceApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        // ---------------- LOGIN ----------------
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid payload");

            try
            {
                var result = await _auth.LoginAsync(dto);

                if (result == null)
                    return Unauthorized("Invalid username or password");
                var isHttps = HttpContext.Request.IsHttps;
                Response.Cookies.Append(
                    "refreshToken",
                    result.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = isHttps,
                        SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
                        Path = "/",
                        Expires = result.RefreshTokenExpiresAt,
                    }
                );

                return Ok(
                    new
                    {
                        AccessToken = result.AccessToken,
                        AccessTokenExpiresAt = result.AccessTokenExpiresAt,
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("LOGIN ERROR: " + ex);
                return StatusCode(500, "Internal server error during login");
            }
        }

        // ---------------- REFRESH ----------------
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                    return Unauthorized("Missing refresh token");

                var result = await _auth.RefreshTokenAsync(refreshToken);

                if (result == null)
                    return Unauthorized("Invalid refresh token");
                var isHttps = HttpContext.Request.IsHttps;
                Response.Cookies.Append(
                    "refreshToken",
                    result.RefreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = isHttps,
                        SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
                        Path = "/",
                        Expires = result.RefreshTokenExpiresAt,
                    }
                );

                return Ok(
                    new
                    {
                        AccessToken = result.AccessToken,
                        AccessTokenExpiresAt = result.AccessTokenExpiresAt,
                    }
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("REFRESH ERROR: " + ex);
                return StatusCode(500, "Internal server error during refresh");
            }
        }

        // ---------------- LOGOUT ----------------
        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                {
                    await _auth.LogoutAsync(refreshToken);
                }
                var isHttps = HttpContext.Request.IsHttps;
                Response.Cookies.Delete(
                    "refreshToken",
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = isHttps,
                        SameSite = isHttps ? SameSiteMode.None : SameSiteMode.Lax,
                        Path = "/",
                    }
                );

                return Ok(new { message = "Logged out" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("LOGOUT ERROR: " + ex);
                return StatusCode(500, "Internal server error during logout");
            }
        }

        // ---------------- ME ----------------
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var employeeId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (employeeId == null)
                return Unauthorized();

            return Ok(new { EmployeeId = employeeId, Role = role });
        }
    }
}
