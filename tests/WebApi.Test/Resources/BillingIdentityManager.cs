using BarberBoss.Domain.Entities.Billing;

namespace WebApi.Tests.Resources
{
    public class BillingIdentityManager
    {
        private Billing _billing;

        public BillingIdentityManager(Billing billing)
        {
            _billing = billing;
        }

        public long GetBillingId() => _billing.Id;

        public DateTime GetDate() => _billing.Date;
    }
}
