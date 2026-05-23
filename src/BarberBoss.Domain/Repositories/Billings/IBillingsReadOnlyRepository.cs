using BarberBoss.Domain.DTOs;
using BarberBoss.Domain.Entities.Billing;

namespace BarberBoss.Domain.Repositories.Billings
{
    public interface IBillingsReadOnlyRepository
    {
        Task<PageResultDTO<Billing>> GetAll(int page, int pageSize, Entities.User user);
        Task<Billing?> GetById(long id, Entities.User user);
        Task<List<Billing>> FilterByWeek(DateOnly date, Entities.User user);
    }
}
