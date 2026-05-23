using BarberBoss.Domain.Entities.Billing;

namespace BarberBoss.Domain.Repositories.Billings
{
    public interface IBillingsWriteOnlyRepository
    {
        Task Add(Billing billing);
        Task Delete(long id);
    }
}
