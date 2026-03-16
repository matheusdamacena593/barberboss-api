using BarberBoss.Communication.Responses;
using BarberBoss.Domain.DTOs;

namespace BarberBoss.Application.UseCases.Billings.GetAll
{
    public interface IGetAllBillingsUseCase
    {
        Task<PageResultDTO<ResponseBillingJson>> Execute(int page, int pageSize);
    }
}
