using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IDesignationRepo
    {
        Task<List<Designation>> GetAllAsync();
        Task<Designation> GetByIdAsync(int id);
        Task<Designation> CreateAsync(Designation entity);
        Task UpdateAsync(Designation entity);
        Task DeleteAsync(Designation entity);
    }
}
