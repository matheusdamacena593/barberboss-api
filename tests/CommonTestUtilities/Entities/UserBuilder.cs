using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using Bogus;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities
{
    public class UserBuilder
    {
        public static User Build(string role = Roles.TEAM_MEMBER)
        {
            var passwordEncripter = new PasswordEncrypterBuilder().Build();

            var user = new Faker<User>()
                .RuleFor(user => user.Id, _ => 1)
                .RuleFor(user => user.Name, faker => faker.Person.FirstName)
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Email))
                .RuleFor(user => user.Password, (_, user) => passwordEncripter.Encrypt(user.Email))
                .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(user => user.Role, _ => role);

            return user;
        }
    }
}
