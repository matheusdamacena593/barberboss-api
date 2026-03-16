using BarberBoss.Application.UseCases.Billings;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Billings.Register
{
    public class RegisterBillingValidatorTests
    {
        [Fact]
        public void Success()
        {
            // Arrange
            var validator = new BillingValidator();
            var request = RequestRegisterBillingsJsonBuilder.Build();

            // Act
            var result = validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
