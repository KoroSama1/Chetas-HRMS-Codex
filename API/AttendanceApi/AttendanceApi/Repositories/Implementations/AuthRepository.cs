using AttendanceApi.Data;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AttendanceApi.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _db;

        public AuthRepository(AppDbContext db) => _db = db;

        public async Task<Employee?> GetByUsernameAsync(string username)
        {
            return await _db
                .Employees.Include(e => e.Role)
                .SingleOrDefaultAsync(e => e.Username == username);
        }

        public async Task<Employee?> GetByEmployeeIdAsync(int employeeId)
        {
            return await _db
                .Employees.Include(e => e.Role)
                .SingleOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string tokenId)
        {
            return await _db.RefreshTokens.SingleOrDefaultAsync(t => t.TokenId == tokenId);
        }

        public async Task AddRefreshTokenAsync(RefreshToken token)
        {
            await _db.RefreshTokens.AddAsync(token);
        }

        public Task UpdateRefreshTokenAsync(RefreshToken token)
        {
            _db.RefreshTokens.Update(token);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
