using BarberBoss.Exception.ExceptionsBase;
using System.Net;

namespace BarberBoss.Exception.ExceptionBase
{
    public class InvalidLoginException : BarberBossException
    {
        public InvalidLoginException() : base(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID) { }

        public override int StatusCode => (int)HttpStatusCode.Unauthorized;

        public override List<string> GetErrors()
        {
            return [Message];
        }
    }
}
