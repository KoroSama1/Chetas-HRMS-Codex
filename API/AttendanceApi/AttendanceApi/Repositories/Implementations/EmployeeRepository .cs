using AttendanceApi.Data;
using AttendanceApi.DTOs.ProfileInfo;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeProfileDto?> GetProfileByIdAsync(int employeeId)
        {
            return await _context
                .Employees.Where(e => e.EmployeeId == employeeId && !e.IsDeleted)
                .Select(e => new EmployeeProfileDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Username = e.Username,
                    EmailId = e.EmailId,
                    PhoneNumber = e.PhoneNumber,
                    DateOfJoining = e.DateOfJoining,
                    Region = e.Region.RegionName,
                    Department = e.Department.DepartmentName,
                    Designation = e.Designation.DesignationName,
                    Role = e.Role.RoleName,
                    EmployeePhotoPath = e.EmployeePhotoPath,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeProfileDto>> GetAllProfilesAsync()
        {
            return await _context
                .Employees.Where(e => !e.IsDeleted)
                .Select(e => new EmployeeProfileDto
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeCode = e.EmployeeCode,
                    FullName = e.FullName,
                    Username = e.Username,
                    EmailId = e.EmailId,
                    PhoneNumber = e.PhoneNumber,
                    DateOfJoining = e.DateOfJoining,
                    Region = e.Region.RegionName,
                    RegionId = e.RegionId,
                    Department = e.Department.DepartmentName,
                    DepartmentId = e.DepartmentId,
                    Designation = e.Designation.DesignationName,
                    DesignationId = e.DesignationId,
                    Role = e.Role.RoleName,
                    RoleId = e.RoleId,
                    EmployeePhotoPath = e.EmployeePhotoPath,
                })
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int employeeId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e =>
                e.EmployeeId == employeeId && !e.IsDeleted
            );
        }

        //public async Task<EmployeeProfileDto?> GetProfileByIdAsync(int employeeId)
        //{
        //    return await _context.Employees
        //        .Where(e => e.EmployeeId == employeeId)
        //        .Select(e => new EmployeeProfileDto
        //        {
        //            EmployeeId = e.EmployeeId,
        //            EmployeeCode = e.EmployeeCode,
        //            FullName = e.FullName,
        //            Username = e.Username,
        //            EmailId = e.EmailId,
        //            PhoneNumber = e.PhoneNumber,
        //            DateOfJoining = e.DateOfJoining,

        //            Region = e.Region.RegionName,
        //            Department = e.Department.DepartmentName,
        //            Designation = e.Designation.DesignationName,
        //            Role = e.Role.RoleName,
        //            EmployeePhotoPath=e.EmployeePhotoPath,
        //        })
        //        .FirstOrDefaultAsync();
        //}

        //public async Task<List<EmployeeProfileDto>> GetAllProfilesAsync()
        //{
        //    return await _context.Employees
        //        .Select(e => new EmployeeProfileDto
        //        {
        //            EmployeeId = e.EmployeeId,
        //            EmployeeCode = e.EmployeeCode,
        //            FullName = e.FullName,
        //            Username = e.Username,
        //            EmailId = e.EmailId,
        //            PhoneNumber = e.PhoneNumber,
        //            DateOfJoining = e.DateOfJoining,
        //            Region = e.Region.RegionName,
        //            Department = e.Department.DepartmentName,
        //            Designation = e.Designation.DesignationName,
        //            Role = e.Role.RoleName,
        //            EmployeePhotoPath = e.EmployeePhotoPath
        //        })
        //        .ToListAsync();
        //}
        public Task<bool> UsernameExistsAsync(string username) =>
            _context.Employees.AnyAsync(e => e.Username == username && !e.IsDeleted);

        public Task<bool> EmailExistsAsync(string email) =>
            _context.Employees.AnyAsync(e => e.EmailId == email && !e.IsDeleted);

        public Task<bool> PhoneExistsAsync(string phone) =>
            _context.Employees.AnyAsync(e => e.PhoneNumber == phone && !e.IsDeleted);

        public Task<bool> EmployeeCodeExistsAsync(int employeeCode) =>
            _context.Employees.AnyAsync(e => e.EmployeeCode == employeeCode && !e.IsDeleted);
    }
}
