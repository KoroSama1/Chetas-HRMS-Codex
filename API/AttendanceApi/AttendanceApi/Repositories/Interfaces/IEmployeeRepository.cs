using AttendanceApi.DTOs.ProfileInfo;
using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task<EmployeeProfileDto?> GetProfileByIdAsync(int employeeId);
        Task<List<EmployeeProfileDto>> GetAllProfilesAsync();

        Task<Employee?> GetByIdAsync(int employeeId);

        ////Validations
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phone);
        Task<bool> EmployeeCodeExistsAsync(int employeeCode);
    }
}
