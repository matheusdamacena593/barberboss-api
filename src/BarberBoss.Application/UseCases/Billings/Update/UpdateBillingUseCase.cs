using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Update
{
    public class UpdateBillingUseCase : IUpdateBillingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBillingsUpdateOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public UpdateBillingUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBillingsUpdateOnlyRepository repository,
            ILoggedUser loggedUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task Execute(long id, RequestBillingJson request)
        {
            await ValidateAsync(request);

            var loggedUser = await _loggedUser.Get();

            var billing = await _repository.GetById(id, loggedUser);

            if (billing is null)
            {
                throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);
            }

            _mapper.Map(request, billing);
            billing.UpdatedAt = DateTime.UtcNow;

            _repository.Update(billing);

            await _unitOfWork.Commit();
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
