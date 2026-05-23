using BarberBoss.Domain.Entities.Billing;

namespace BarberBoss.Domain.Repositories.Billings
{
    public interface IBillingsUpdateOnlyRepository
    {
        Task<Billing?> GetById(long id, Entities.User user);
        void Update(Billing billing);
    }
}
