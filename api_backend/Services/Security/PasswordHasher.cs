using System.Security.Cryptography;
using System.Text;

namespace dotnet.Services.Security
{
    /// <summary>
    /// PBKDF2 password hasher with per-user salt.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        // PUBLIC_INTERFACE
        public (string hash, string salt) HashPassword(string password)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            var hashBytes = PBKDF2(password, saltBytes, Iterations, KeySize);
            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        // PUBLIC_INTERFACE
        public bool Verify(string password, string hash, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var hashBytes = PBKDF2(password, saltBytes, Iterations, KeySize);
            var existingHashBytes = Convert.FromBase64String(hash);
            return CryptographicOperations.FixedTimeEquals(hashBytes, existingHashBytes);
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int keySize)
        {
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(keySize);
        }
    }
}
