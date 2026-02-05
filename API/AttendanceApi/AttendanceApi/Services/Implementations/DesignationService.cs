using System.Net;
using AttendanceApi.DTOs.Masters;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;

namespace AttendanceApi.Services.Implementations
{
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepo _repo;

        public DesignationService(IDesignationRepo repo) => _repo = repo;

        public async Task<List<MasterDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(d => new MasterDto
                {
                    Id = d.DesignationId,
                    Name = d.DesignationName,
                })
                .ToList();
        }

        public async Task<MasterDto> CreateAsync(CreateMasterDto dto)
        {
            var desig = new Designation { DesignationName = dto.Name };
            await _repo.CreateAsync(desig);
            return new MasterDto { Id = desig.DesignationId, Name = desig.DesignationName };
        }

        public async Task<MasterDto> UpdateAsync(int id, UpdateMasterDto dto)
        {
            var desig = await _repo.GetByIdAsync(id);
            if (desig == null)
                throw new AppException("Designation not found", HttpStatusCode.NotFound);

            desig.DesignationName = dto.Name;
            await _repo.UpdateAsync(desig);
            return new MasterDto { Id = desig.DesignationId, Name = desig.DesignationName };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var desig = await _repo.GetByIdAsync(id);
            if (desig == null)
                throw new AppException("Designation not found", HttpStatusCode.NotFound);

            if (desig.Employees != null && desig.Employees.Any())
                throw new AppException(
                    "Cannot delete designation. Employees are assigned to it.",
                    HttpStatusCode.Conflict
                );

            await _repo.DeleteAsync(desig);
            return true;
        }
    }
}
