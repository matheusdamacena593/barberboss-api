using BarberBoss.Domain.DTOs;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings
{
    public interface IBillingsReadOnlyRepository
    {
        Task<PageResultDTO<Billing>> GetAll(int page, int pageSize);
        Task<Billing?> GetById(Guid id);
        Task<List<Billing>> FilterByWeek(DateOnly date);
    }
}
