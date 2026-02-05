using System.Net;
using AttendanceApi.DTOs.HRDto;
using AttendanceApi.DTOs.ProfileInfo;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;
using BCrypt.Net;

namespace AttendanceApi.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IFileStorageService _fileStorage;

        public EmployeeService(IEmployeeRepository employeeRepo, IFileStorageService fileStorage)
        {
            _employeeRepo = employeeRepo;
            _fileStorage = fileStorage;
        }

        public async Task CreateEmployeeAsync(CreateEmployeeRequestDto request)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var employee = new Employee
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                EmployeeCode = request.EmployeeCode,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                EmailId = request.EmailId.Trim().ToLower(),
                DateOfJoining = request.DateOfJoining,
                RegionId = request.RegionId,
                DepartmentId = request.DepartmentId,
                DesignationId = request.DesignationId,
                RoleId = request.RoleId,
            };

            await _employeeRepo.AddAsync(employee); // ID generated

            if (!string.IsNullOrWhiteSpace(request.PhotoBase64))
            {
                var imageBytes = Convert.FromBase64String(request.PhotoBase64.Split(',')[1]);

                var photoPath = await _fileStorage.SaveEmployeeProfilePhotoAsync(
                    employee.EmployeeId,
                    imageBytes
                );

                employee.EmployeePhotoPath = photoPath;

                await _employeeRepo.UpdateAsync(employee);
            }
        }

        public async Task<EmployeeProfileDto?> GetMyProfileAsync(int employeeId, string baseUrl)
        {
            var profile = await _employeeRepo.GetProfileByIdAsync(employeeId);

            if (profile?.EmployeePhotoPath != null)
            {
                profile.EmployeePhotoPath = $"{baseUrl}/uploads{profile.EmployeePhotoPath}";
            }

            return profile;
        }

        public async Task<List<EmployeeProfileDto>> GetAllProfilesAsync(string baseUrl)
        {
            var profiles = await _employeeRepo.GetAllProfilesAsync();

            // Update photo paths to full URLs
            profiles.ForEach(p =>
            {
                if (!string.IsNullOrEmpty(p.EmployeePhotoPath))
                {
                    p.EmployeePhotoPath = $"{baseUrl}/uploads{p.EmployeePhotoPath}";
                }
            });

            return profiles;
        }

        public async Task UpdateEmployeeAsync(int employeeId, UpdateEmployeeRequestDto request)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);

            if (employee == null)
                throw new AppException("Employee not found", HttpStatusCode.NotFound);

            employee.FullName = request.FullName;
            employee.PhoneNumber = request.PhoneNumber;
            employee.EmailId = request.EmailId.Trim().ToLower();
            employee.RegionId = request.RegionId;
            employee.DepartmentId = request.DepartmentId;
            employee.DesignationId = request.DesignationId;
            employee.RoleId = request.RoleId;

            await _employeeRepo.UpdateAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);

            if (employee == null)
                throw new AppException("Employee not found", HttpStatusCode.NotFound);

            // ✅ Soft delete
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.UtcNow;

            await _employeeRepo.UpdateAsync(employee);
        }

        ///Validation
        public async Task<FieldValidationResultDto> ValidateUsernameAsync(string value)
        {
            value = value.Trim().ToLower();

            var exists = await _employeeRepo.UsernameExistsAsync(value);

            return new FieldValidationResultDto
            {
                Field = "username",
                IsValid = !exists,
                Message = exists ? "Username already exists" : "Username is available",
            };
        }

        public async Task<FieldValidationResultDto> ValidateEmailAsync(string value)
        {
            value = value.Trim().ToLower();

            var exists = await _employeeRepo.EmailExistsAsync(value);

            return new FieldValidationResultDto
            {
                Field = "email",
                IsValid = !exists,
                Message = exists ? "Email already exists" : "Email is available",
            };
        }

        public async Task<FieldValidationResultDto> ValidatePhoneAsync(string value)
        {
            value = value.Trim();

            var exists = await _employeeRepo.PhoneExistsAsync(value);

            return new FieldValidationResultDto
            {
                Field = "phone",
                IsValid = !exists,
                Message = exists ? "Phone number already exists" : "Phone number is available",
            };
        }

        public async Task<FieldValidationResultDto> ValidateEmployeeCodeAsync(int value)
        {
            var exists = await _employeeRepo.EmployeeCodeExistsAsync(value);

            return new FieldValidationResultDto
            {
                Field = "employeeCode",
                IsValid = !exists,
                Message = exists ? "Employee code already exists" : "Employee code is available",
            };
        }
    }
}
