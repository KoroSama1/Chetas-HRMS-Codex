using AttendanceApi.DTOs.Auth;
using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Employee?> GetByUsernameAsync(string username);
        Task<Employee?> GetByEmployeeIdAsync(int employeeId);

        Task<RefreshToken?> GetRefreshTokenAsync(string tokenId);

        Task AddRefreshTokenAsync(RefreshToken token);
        Task UpdateRefreshTokenAsync(RefreshToken token);

        Task SaveChangesAsync();
    }
}
