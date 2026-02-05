using AttendanceApi.Data;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class RegionRepo : IRegionRepo
    {
        private readonly AppDbContext _context;

        public RegionRepo(AppDbContext context) => _context = context;

        public async Task<List<Region>> GetAllAsync() => await _context.Regions.ToListAsync();

        public async Task<Region> GetByIdAsync(int id) =>
            await _context
                .Regions.Include(r => r.Employees) // include employees for safe delete
                .FirstOrDefaultAsync(r => r.RegionId == id);

        public async Task<Region> CreateAsync(Region entity)
        {
            _context.Regions.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Region entity)
        {
            _context.Regions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Region entity)
        {
            _context.Regions.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
