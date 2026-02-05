using AttendanceApi.Data;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class RoleRepo : IRoleRepo
    {
        private readonly AppDbContext _context;

        public RoleRepo(AppDbContext context) => _context = context;

        public async Task<List<Role>> GetAllAsync() => await _context.Roles.ToListAsync();

        public async Task<Role> GetByIdAsync(int id) =>
            await _context
                .Roles.Include(r => r.Employees) // include employees for safe delete check
                .FirstOrDefaultAsync(r => r.RoleId == id);

        public async Task<Role> CreateAsync(Role entity)
        {
            _context.Roles.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Role entity)
        {
            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role entity)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
