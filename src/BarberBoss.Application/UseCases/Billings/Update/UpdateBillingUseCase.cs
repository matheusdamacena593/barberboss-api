using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Billings.Update
{
    public class UpdateBillingUseCase : IUpdateBillingUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBillingsUpdateOnlyRepository _repository;

        public UpdateBillingUseCase(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBillingsUpdateOnlyRepository repository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Execute(Guid id, RequestBillingJson request)
        {
            await ValidateAsync(request);

            var billing = await _repository.GetById(id);

            if (billing is null)
            {
                throw new NotFoundException(ResourceErrorMessages.BILLING_NOT_FOUND);
            }

            _mapper.Map(request, billing);

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
