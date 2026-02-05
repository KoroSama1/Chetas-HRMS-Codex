using AttendanceApi.DTOs.HRDto;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IRegionRepository
    {
        Task<List<RegionDto>> GetAllAsync();
    }
}
