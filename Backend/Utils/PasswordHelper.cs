using System.Security.Cryptography;
using System.Text;
using System;

namespace PawpalBackend.Utils
{
    public class PasswordHelper
    {
        public static void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (string.IsNullOrEmpty(password))  // Check for null or empty password
            {
                throw new ArgumentException("Password cannot be null or empty", nameof(password));
            }

            using (var hmac = new HMACSHA512())
            {
                var saltBytes = hmac.Key;
                var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                passwordSalt = Convert.ToBase64String(saltBytes);
                passwordHash = Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(computedHash) == storedHash;
            }
        }
    }
}