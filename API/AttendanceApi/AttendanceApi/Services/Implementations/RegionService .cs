using System.Net;
using AttendanceApi.DTOs.HRDto;
using AttendanceApi.DTOs.Masters;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;

namespace AttendanceApi.Services.Implementations
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _repository;
        private readonly IRegionRepo _repo;

        public RegionService(IRegionRepository repo1, IRegionRepo repo)
        {
            _repository = repo1;
            _repo = repo;
        }

        public Task<List<RegionDto>> GetAllRegionAsync()
        {
            return _repository.GetAllAsync();
        }

        public async Task<List<MasterDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(r => new MasterDto { Id = r.RegionId, Name = r.RegionName })
                .ToList();
        }

        public async Task<MasterDto> CreateAsync(CreateMasterDto dto)
        {
            var region = new Region { RegionName = dto.Name };
            await _repo.CreateAsync(region);
            return new MasterDto { Id = region.RegionId, Name = region.RegionName };
        }

        public async Task<MasterDto> UpdateAsync(int id, UpdateMasterDto dto)
        {
            var region = await _repo.GetByIdAsync(id);
            if (region == null)
                throw new AppException("Region not found", HttpStatusCode.NotFound);

            region.RegionName = dto.Name;
            await _repo.UpdateAsync(region);
            return new MasterDto { Id = region.RegionId, Name = region.RegionName };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var region = await _repo.GetByIdAsync(id);
            if (region == null)
                throw new AppException("Region not found", HttpStatusCode.NotFound);

            if (region.Employees != null && region.Employees.Any())
                throw new AppException(
                    "Cannot delete region. Employees are assigned to it.",
                    HttpStatusCode.Conflict
                );

            await _repo.DeleteAsync(region);
            return true;
        }
    }
}
