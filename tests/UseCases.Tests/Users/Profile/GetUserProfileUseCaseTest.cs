using BarberBoss.Application.UseCases.Users.Profile;
using BarberBoss.Domain.Entities;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using FluentAssertions;

namespace UseCases.Tests.Users.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            var loggedUser = UserBuilder.Build();
            var useCase = CreateUseCase(loggedUser);

            var result = await useCase.Execute();

            result.Should().NotBeNull();
            result.Name.Should().Be(loggedUser.Name);
            result.Email.Should().Be(loggedUser.Email);
        }

        private GetUserProfileUseCase CreateUseCase(User user)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(user);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }
    }
}
