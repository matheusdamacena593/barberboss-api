using BarberBoss.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography
{
    public class PasswordEncrypterBuilder
    {
        private readonly Mock<IPasswordEncripter> _mock;

        public PasswordEncrypterBuilder()
        {
            _mock = new Mock<IPasswordEncripter>();

            _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("dsadaADS@!!!342343");
        }

        public PasswordEncrypterBuilder VerifyPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password) == false)
            {
                _mock.Setup(passwordEncrypter => passwordEncrypter.VerifyPassword(password, It.IsAny<string>())).Returns(true);
            }

            return this;
        }

        public IPasswordEncripter Build() => _mock.Object;
    }
}
