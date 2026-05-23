using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities.Billing;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Register
{
    public class RegisterBillingUseCase : IRegisterBillingUseCase
    {
        private readonly IBillingsWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public RegisterBillingUseCase(
            IBillingsWriteOnlyRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseBillingJson> Execute(RequestBillingJson request)
        {
            await ValidateAsync(request);

            var loggedUser = await _loggedUser.Get();

            var billing = _mapper.Map<Billing>(request);
            billing.UserId = loggedUser.Id;

            await _repository.Add(billing);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseBillingJson>(billing);
        }

        private async Task ValidateAsync(RequestBillingJson request)
        {
            var validator = new BillingValidator();

            var result = await validator.ValidateAsync(request);

            if (!result.IsValid)
            {
                var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
