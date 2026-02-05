using AttendanceApi.Data;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class DesignationRepo : IDesignationRepo
    {
        private readonly AppDbContext _context;

        public DesignationRepo(AppDbContext context) => _context = context;

        public async Task<List<Designation>> GetAllAsync() =>
            await _context.Designations.ToListAsync();

        public async Task<Designation> GetByIdAsync(int id) =>
            await _context
                .Designations.Include(d => d.Employees) // include employees for safe delete check
                .FirstOrDefaultAsync(d => d.DesignationId == id);

        public async Task<Designation> CreateAsync(Designation entity)
        {
            _context.Designations.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Designation entity)
        {
            _context.Designations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Designation entity)
        {
            _context.Designations.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
