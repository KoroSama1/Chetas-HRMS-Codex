using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IDepartmentRepo
    {
        Task<List<Department>> GetAllAsync();
        Task<Department> GetByIdAsync(int id);
        Task<Department> CreateAsync(Department entity);
        Task UpdateAsync(Department entity);
        Task DeleteAsync(Department entity);
    }
}
