using AttendanceApi.DTOs.Masters;

namespace AttendanceApi.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<MasterDto>> GetAllAsync();
        Task<MasterDto> CreateAsync(CreateMasterDto dto);
        Task<MasterDto> UpdateAsync(int id, UpdateMasterDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
