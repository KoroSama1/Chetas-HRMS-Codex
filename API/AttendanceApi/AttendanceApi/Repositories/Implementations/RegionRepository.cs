using AttendanceApi.Data;
using AttendanceApi.DTOs.HRDto;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class RegionRepository : IRegionRepository
    {
        private readonly AppDbContext _context;

        public RegionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RegionDto>> GetAllAsync()
        {
            return await _context
                .Regions.Select(r => new RegionDto
                {
                    RegionId = r.RegionId,
                    RegionName = r.RegionName,
                })
                .ToListAsync();
        }
    }
}
