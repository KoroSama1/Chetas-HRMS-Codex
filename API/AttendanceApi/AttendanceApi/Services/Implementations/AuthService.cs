using System.Net;
using AttendanceApi.DTOs.Auth;
using AttendanceApi.Entities;
using AttendanceApi.Repositories.Interfaces;
using AttendanceApi.Services.Interfaces;
using AttendanceApi.Utils;
using System.IdentityModel.Tokens.Jwt;

namespace AttendanceApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly JwtHelper _jwt;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository repo, JwtHelper jwt, ILogger<AuthService> logger)
        {
            _repo = repo;
            _jwt = jwt;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByUsernameAsync(dto.Username);
            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
                throw new AppException("Invalid username or password", HttpStatusCode.Unauthorized);

            var (accessToken, expiresAt) = _jwt.GenerateAccessToken(user);
            LogAccessTokenClaims("login", accessToken);
            var refreshSecret = JwtHelper.GenerateRefreshToken();

            var createdAt = IstTimeProvider.Now;
            var maxSessionLifetime = TimeSpan.FromDays(14); // use a constant/config if possible
            var sessionExpiry = createdAt + maxSessionLifetime;

            var refreshToken = new RefreshToken
            {
                TokenId = Guid.NewGuid().ToString("N"),
                TokenHash = PasswordHasher.Hash(refreshSecret),
                EmployeeId = user.EmployeeId,
                CreatedAt = createdAt,
                ExpiresAt = sessionExpiry, // use absolute expiry
                IsRevoked = false,
            };

            await _repo.AddRefreshTokenAsync(refreshToken);
            await _repo.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = expiresAt,
                RefreshToken = $"{refreshToken.TokenId}:{refreshSecret}",
                RefreshTokenExpiresAt = sessionExpiry, // match cookie expiry
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var parts = refreshToken.Split(':', 2);
            if (parts.Length != 2)
                throw new AppException("Invalid refresh token", HttpStatusCode.Unauthorized);

            var stored = await _repo.GetRefreshTokenAsync(parts[0]);
            if (stored == null || stored.IsRevoked || stored.ExpiresAt < IstTimeProvider.Now)
                throw new AppException(
                    "Invalid or expired refresh token",
                    HttpStatusCode.Unauthorized
                );

            if (!PasswordHasher.Verify(parts[1], stored.TokenHash))
            {
                stored.IsRevoked = true;
                await _repo.UpdateRefreshTokenAsync(stored);
                await _repo.SaveChangesAsync();
                throw new AppException("Invalid refresh token", HttpStatusCode.Unauthorized);
            }

            // absolute session expiry
            var maxSessionLifetime = TimeSpan.FromDays(14);
            var sessionExpiry = stored.CreatedAt + maxSessionLifetime;

            if (IstTimeProvider.Now >= sessionExpiry)
            {
                stored.IsRevoked = true;
                await _repo.UpdateRefreshTokenAsync(stored);
                await _repo.SaveChangesAsync();
                throw new AppException(
                    "Refresh token session expired",
                    HttpStatusCode.Unauthorized
                );
            }

            var user =
                await _repo.GetByEmployeeIdAsync(stored.EmployeeId)
                ?? throw new AppException("User not found", HttpStatusCode.Unauthorized);

            var (accessToken, expiresAt) = _jwt.GenerateAccessToken(user);
            LogAccessTokenClaims("refresh", accessToken);
            var newSecret = JwtHelper.GenerateRefreshToken();

            // ================= GPT CHANGE START =================
            // Rotate refresh token by UPDATING SAME ROW
            // No new DB row is created

            stored.TokenId = Guid.NewGuid().ToString("N"); // GPT CHANGE: replace token id
            stored.TokenHash = PasswordHasher.Hash(newSecret); // GPT CHANGE: replace token hash
            stored.IsRevoked = false; // GPT CHANGE: keep session alive
            stored.ExpiresAt = sessionExpiry; // GPT CHANGE: absolute expiry unchanged

            await _repo.UpdateRefreshTokenAsync(stored); // GPT CHANGE: update instead of insert
            await _repo.SaveChangesAsync();
            // ================= GPT CHANGE END =================

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = expiresAt,
                RefreshToken = $"{stored.TokenId}:{newSecret}", // GPT CHANGE: return updated token
                RefreshTokenExpiresAt = sessionExpiry,
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var parts = refreshToken.Split(':', 2);
            if (parts.Length != 2)
                return;

            var stored = await _repo.GetRefreshTokenAsync(parts[0]);
            if (stored == null)
                return;

            stored.IsRevoked = true;
            await _repo.UpdateRefreshTokenAsync(stored);
            await _repo.SaveChangesAsync();
        }

        private void LogAccessTokenClaims(string source, string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            _logger.LogInformation(
                "JWT issued via {Source}. Issuer={Issuer} Audience={Audience} ValidFrom={ValidFrom:o} ValidTo={ValidTo:o}",
                source,
                token.Issuer,
                string.Join(",", token.Audiences),
                token.ValidFrom,
                token.ValidTo
            );
        }
    }
}
