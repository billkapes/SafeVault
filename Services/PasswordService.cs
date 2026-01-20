using System.Security.Cryptography;
using System.Text;

namespace SafeVault.Services
{
    public static class PasswordService
    {
        // PBKDF2 configuration
        private const int SaltSize = 16;          // 128-bit salt
        private const int HashSize = 32;          // 256-bit hash
        private const int Iterations = 100_000;   // Strong default for PBKDF2

        // Generates a random salt
        public static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        // Hashes a password using PBKDF2-HMAC-SHA256
        public static byte[] HashPassword(string password, byte[] salt)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256
            );

            return pbkdf2.GetBytes(HashSize);
        }

        // Verifies a password by hashing and comparing
        public static bool VerifyPassword(string password, byte[] salt, byte[] storedHash)
        {
            var computedHash = HashPassword(password, salt);

            // Constant-time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
        }
    }
}
