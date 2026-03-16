using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception.ExceptionsBase;
using PdfSharp.Drawing;

namespace BarberBoss.Application.UseCases.Billings.Register
{
    public class RegisterBillingUseCase : IRegisterBillingUseCase
    {
        private readonly IBillingsWriteOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RegisterBillingUseCase(
            IBillingsWriteOnlyRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseBillingJson> Execute(RequestBillingJson request)
        {
            await ValidateAsync(request);

            var entity = _mapper.Map<Billing>(request);

            await _repository.Add(entity);

            await _unitOfWork.Commit();

            return _mapper.Map<ResponseBillingJson>(entity);
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
