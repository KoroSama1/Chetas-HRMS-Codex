using AttendanceApi.DTOs.HRDto;
using AttendanceApi.DTOs.Masters;

namespace AttendanceApi.Services.Interfaces
{
    public interface IRegionService
    {
        Task<List<RegionDto>> GetAllRegionAsync();

        //////////////////////////
        Task<List<MasterDto>> GetAllAsync();
        Task<MasterDto> CreateAsync(CreateMasterDto dto);
        Task<MasterDto> UpdateAsync(int id, UpdateMasterDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
