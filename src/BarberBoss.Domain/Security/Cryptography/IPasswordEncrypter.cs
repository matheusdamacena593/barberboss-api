namespace BarberBoss.Domain.Security.Cryptography
{
    public interface IPasswordEncripter
    {
        string Encrypt(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}
