using System.Security.Cryptography;
using System.Text;

namespace LW_4_3_5_Daryev_PI231.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var providedHashed = HashPassword(providedPassword);
            return hashedPassword == providedHashed;
        }

    }
}
