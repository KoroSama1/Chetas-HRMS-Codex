using AttendanceApi.Entities;

namespace AttendanceApi.Repositories.Interfaces
{
    public interface IRoleRepo
    {
        Task<List<Role>> GetAllAsync();
        Task<Role> GetByIdAsync(int id);
        Task<Role> CreateAsync(Role entity);
        Task UpdateAsync(Role entity);
        Task DeleteAsync(Role entity);
    }
}
