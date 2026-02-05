//using AttendanceApi.DTOs.Auth;

//namespace AttendanceApi.Services.Interfaces
//{
//    public interface IAuthService
//    {
//        LoginResponseDto Login(LoginDto dto);
//        LoginResponseDto RefreshToken(string refreshToken);
//        void Logout(string refreshToken);
//    }

//}
using AttendanceApi.DTOs.Auth;

namespace AttendanceApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto dto);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
    }
}
