using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests
{
    public class RequestRegisterBillingsJsonBuilder
    {
        public static RequestBillingJson Build(Status status = Status.Pago)
        {
            return new Faker<RequestBillingJson>()
                .RuleFor(r => r.BarberName, faker => faker.Person.FullName)
                .RuleFor(r => r.ClientName, faker => faker.Person.FullName)
                .RuleFor(r => r.ServiceName, faker => faker.Commerce.ProductName())
                .RuleFor(r => r.Amount, faker => status == Status.Pago ? faker.Random.Decimal(min: 1, max: 1000) : 0)
                .RuleFor(r => r.PaymentMethod, faker => faker.PickRandom<PaymentMethod>())
                .RuleFor(r => r.Status, _ => status)
                .RuleFor(r => r.Date, faker => faker.Date.Past())
                .RuleFor(r => r.Notes, faker => faker.Commerce.ProductDescription());
        }
    }
}
