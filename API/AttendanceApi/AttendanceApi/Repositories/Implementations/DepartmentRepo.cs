using AttendanceApi.Data;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly AppDbContext _context;

        public DepartmentRepo(AppDbContext context) => _context = context;

        public async Task<List<Department>> GetAllAsync() =>
            await _context.Departments.ToListAsync();

        public async Task<Department> GetByIdAsync(int id) =>
            await _context.Departments.FindAsync(id);

        public async Task<Department> CreateAsync(Department entity)
        {
            _context.Departments.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Department entity)
        {
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department entity)
        {
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
