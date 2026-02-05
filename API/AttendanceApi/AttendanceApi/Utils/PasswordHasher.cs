using System.Security.Cryptography;

namespace AttendanceApi.Utils
{
    public class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var derive = new Rfc2898DeriveBytes(
                password,
                16,
                100_000,
                HashAlgorithmName.SHA256
            );
            var salt = derive.Salt;
            var key = derive.GetBytes(32);
            return Convert.ToBase64String(salt.Concat(key).ToArray());
        }

        public static bool Verify(string password, string hash)
        {
            var bytes = Convert.FromBase64String(hash);
            var salt = bytes[..16];
            var storedKey = bytes[16..];
            using var derive = new Rfc2898DeriveBytes(
                password,
                salt,
                100_000,
                HashAlgorithmName.SHA256
            );
            var computedKey = derive.GetBytes(32);
            return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
        }
    }
}
