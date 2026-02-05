using System.Net;
using AttendanceApi.DTOs.Masters;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;

namespace AttendanceApi.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _repo;

        public RoleService(IRoleRepo repo) => _repo = repo;

        public async Task<List<MasterDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(r => new MasterDto { Id = r.RoleId, Name = r.RoleName }).ToList();
        }

        public async Task<MasterDto> CreateAsync(CreateMasterDto dto)
        {
            var role = new Role { RoleName = dto.Name };
            await _repo.CreateAsync(role);
            return new MasterDto { Id = role.RoleId, Name = role.RoleName };
        }

        public async Task<MasterDto> UpdateAsync(int id, UpdateMasterDto dto)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null)
                throw new AppException("Role not found", HttpStatusCode.NotFound);

            role.RoleName = dto.Name;
            await _repo.UpdateAsync(role);
            return new MasterDto { Id = role.RoleId, Name = role.RoleName };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _repo.GetByIdAsync(id);
            if (role == null)
                throw new AppException("Role not found", HttpStatusCode.NotFound);

            if (role.Employees != null && role.Employees.Any())
                throw new AppException(
                    "Cannot delete role. Employees are assigned to it.",
                    HttpStatusCode.Conflict
                );

            await _repo.DeleteAsync(role);
            return true;
        }
    }
}
