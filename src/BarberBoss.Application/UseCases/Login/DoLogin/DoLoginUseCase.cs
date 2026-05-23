using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.User;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly IPasswordEncripter _passwordEncripter;
        private readonly IAccessTokenGenerator _tokenGenerator;

        public DoLoginUseCase(
            IUserReadOnlyRepository userReadOnlyRepository,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator tokenGenerator)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _passwordEncripter = passwordEncripter;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
        {
            var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);

            if (user is null)
            {
                throw new InvalidLoginException();
            }

            var passwordMatch = _passwordEncripter.VerifyPassword(request.Password, user.Password);

            if (passwordMatch is false)
            {
                throw new InvalidLoginException();
            }

            return new ResponseRegisteredUserJson
            {
                Name = user.Name,
                Token = _tokenGenerator.Generate(user)
            };
        }
    }
}
