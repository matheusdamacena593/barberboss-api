using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.DTOs;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Billings.GetAll
{
    public class GetAllBillingsUseCase : IGetAllBillingsUseCase
    {
        private readonly IBillingsReadOnlyRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetAllBillingsUseCase(
            IBillingsReadOnlyRepository repository,
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }
        public async Task<PageResultDTO<ResponseBillingJson>> Execute(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var loggedUser = await _loggedUser.Get();

            var result = await _repository.GetAll(page, pageSize, loggedUser);

            return new PageResultDTO<ResponseBillingJson>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                Items = _mapper.Map<List<ResponseBillingJson>>(result.Items)
            };
        }
    }
}
