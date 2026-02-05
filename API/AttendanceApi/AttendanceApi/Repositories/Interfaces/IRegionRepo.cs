using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IRegionRepo
    {
        Task<List<Region>> GetAllAsync();
        Task<Region> GetByIdAsync(int id);
        Task<Region> CreateAsync(Region entity);
        Task UpdateAsync(Region entity);
        Task DeleteAsync(Region entity);
    }
}
