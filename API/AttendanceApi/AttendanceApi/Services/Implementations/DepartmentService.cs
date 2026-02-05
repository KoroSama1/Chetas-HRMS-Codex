using System.Net;
using AttendanceApi.DTOs.Masters;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;

namespace AttendanceApi.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _repo;

        public DepartmentService(IDepartmentRepo repo) => _repo = repo;

        public async Task<List<MasterDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(d => new MasterDto { Id = d.DepartmentId, Name = d.DepartmentName })
                .ToList();
        }

        public async Task<MasterDto> CreateAsync(CreateMasterDto dto)
        {
            var dep = new Department { DepartmentName = dto.Name };
            await _repo.CreateAsync(dep);
            return new MasterDto { Id = dep.DepartmentId, Name = dep.DepartmentName };
        }

        public async Task<MasterDto> UpdateAsync(int id, UpdateMasterDto dto)
        {
            var dep = await _repo.GetByIdAsync(id);
            if (dep == null)
                throw new AppException("Department not found", HttpStatusCode.NotFound);

            dep.DepartmentName = dto.Name;
            await _repo.UpdateAsync(dep);
            return new MasterDto { Id = dep.DepartmentId, Name = dep.DepartmentName };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dep = await _repo.GetByIdAsync(id);
            if (dep == null)
                throw new AppException("Department not found", HttpStatusCode.NotFound);

            if (dep.Employees != null && dep.Employees.Any())
                throw new AppException(
                    "Cannot delete department. Employees are assigned to it.",
                    HttpStatusCode.Conflict
                );

            await _repo.DeleteAsync(dep);
            return true;
        }
    }
}
