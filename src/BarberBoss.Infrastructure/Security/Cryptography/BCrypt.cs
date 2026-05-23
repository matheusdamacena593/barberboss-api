using BarberBoss.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace BarberBoss.Infrastructure.Security.Cryptography
{
    internal class BCrypt : IPasswordEncripter
    {
        public string Encrypt(string password)
        {
            string passwordHash = BC.HashPassword(password);

            return passwordHash;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BC.Verify(password, passwordHash);
        }
    }
}
