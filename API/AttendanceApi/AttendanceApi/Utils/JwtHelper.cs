using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AttendanceApi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceApi.Utils
{
    public class JwtHelper
    {
        private readonly IConfiguration _config;

        public JwtHelper(IConfiguration config) => _config = config;

        public (string token, DateTime expires) GenerateAccessToken(Employee user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.EmployeeId.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim("Username", user.Username),
            };

            var expires = IstTimeProvider.Now.AddMinutes(
                int.Parse(_config["Jwt:AccessTokenMinutes"])
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        public static string GenerateRefreshToken() =>
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
