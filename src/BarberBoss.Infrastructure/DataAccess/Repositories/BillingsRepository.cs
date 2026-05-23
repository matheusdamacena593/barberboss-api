using BarberBoss.Domain.DTOs;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Entities.Billing;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories
{
    internal class BillingsRepository : IBillingsReadOnlyRepository, IBillingsWriteOnlyRepository, IBillingsUpdateOnlyRepository
    {
        private readonly BarberBossDbContext _dbContext;

        public BillingsRepository(BarberBossDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Billing billing)
        {
            await _dbContext
                .Billings
                .AddAsync(billing);
        }

        public async Task Delete(long id)
        {
            var result = await _dbContext
                .Billings
                .FindAsync(id);

            _dbContext
                .Billings
                .Remove(result);
        }

        public async Task<PageResultDTO<Billing>> GetAll(int page, int pageSize, User user)
        {
            pageSize = Math.Min(pageSize, 50);
            
            var baseQuery = _dbContext.Billings.AsNoTracking().Where(expense => expense.UserId == user.Id);

            var totalItems = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageResultDTO<Billing>
            {
                Page = page,
                PageSize = items.Count,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = items
            };
        }

        async Task<Billing?> IBillingsReadOnlyRepository.GetById(long id, User user)
        {
            return await _dbContext
                .Billings
                .AsNoTracking()
                .FirstOrDefaultAsync(billing => billing.Id == id && billing.UserId == user.Id);
        }

        async Task<Billing?> IBillingsUpdateOnlyRepository.GetById(long id, User user)
        {
            return await _dbContext
               .Billings
               .FirstOrDefaultAsync(billing => billing.Id == id && billing.UserId == user.Id);
        }

        public void Update(Billing billing)
        {
            _dbContext
                .Billings
                .Update(billing);
        }

        public async Task<List<Billing>> FilterByWeek(DateOnly date, User user)
        {
            var dayOfWeek = date.DayOfWeek;

            var diffToMonday = dayOfWeek == DayOfWeek.Sunday ? 6 : ((int)dayOfWeek - 1);

            var startOfWeekDate = date.AddDays(-diffToMonday);
            var endOfWeekDate = startOfWeekDate.AddDays(6);

            var startDate = startOfWeekDate.ToDateTime(TimeOnly.MinValue);
            var endDate = endOfWeekDate.ToDateTime(TimeOnly.MaxValue);

            return await _dbContext
                .Billings
                .AsNoTracking()
                .Where(billing => billing.Date >= startDate && billing.Date <= endDate && billing.UserId == user.Id)
                .OrderBy(billing => billing.Date)
                .ThenBy(billing => billing.ServiceName)
                .ToListAsync();
        }
    }
}
