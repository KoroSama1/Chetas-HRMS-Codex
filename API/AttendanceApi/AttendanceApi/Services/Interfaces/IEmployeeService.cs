using AttendanceApi.DTOs.HRDto;
using AttendanceApi.DTOs.ProfileInfo;

namespace AttendanceApi.Services.Interfaces
{
    public interface IEmployeeService
    {
        //Task<EmployeeProfileDto> GetMyProfileAsync(int employeeId);

        //Task CreateEmployeeAsync(CreateEmployeeRequestDto request);
        Task<EmployeeProfileDto?> GetMyProfileAsync(int employeeId, string baseUrl);
        Task CreateEmployeeAsync(CreateEmployeeRequestDto request);

        Task<List<EmployeeProfileDto>> GetAllProfilesAsync(string baseUrl);

        Task UpdateEmployeeAsync(int employeeId, UpdateEmployeeRequestDto request);
        Task DeleteEmployeeAsync(int employeeId);

        ///Validations
        Task<FieldValidationResultDto> ValidateUsernameAsync(string value);
        Task<FieldValidationResultDto> ValidateEmailAsync(string value);
        Task<FieldValidationResultDto> ValidatePhoneAsync(string value);
        Task<FieldValidationResultDto> ValidateEmployeeCodeAsync(int value);
    }
}
