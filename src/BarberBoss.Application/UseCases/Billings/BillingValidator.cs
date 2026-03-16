using BarberBoss.Communication.Requests;
using FluentValidation;
using BarberBoss.Exception;

namespace BarberBoss.Application.UseCases.Billings
{
    public class BillingValidator : AbstractValidator<RequestBillingJson>
    {
        public BillingValidator()
        {
            RuleFor(billing => billing.Date)
                .NotEmpty().WithMessage(ResourceErrorMessages.DATA_OBRIGATORIA);
            
            RuleFor(billing => billing.BarberName)
                .NotEmpty().WithMessage(ResourceErrorMessages.BARBER_NAME_OBRIGATORIO)
                .MinimumLength(2).WithMessage(ResourceErrorMessages.BARBER_NAME_MINIMO)
                .MaximumLength(80).WithMessage(ResourceErrorMessages.BARBER_NAME_MAXIMO);

            RuleFor(billing => billing.ClientName)
                .NotEmpty().WithMessage(ResourceErrorMessages.CLIENT_NAME_OBRIGATORIO)
                .MinimumLength(2).WithMessage(ResourceErrorMessages.CLIENT_NAME_MINIMO)
                .MaximumLength(120).WithMessage(ResourceErrorMessages.CLIENT_NAME_MAXIMO);
            
            RuleFor(billing => billing.ServiceName)
                .NotEmpty().WithMessage(ResourceErrorMessages.SERVICE_NAME_OBRIGATORIO)
                .MinimumLength(2).WithMessage(ResourceErrorMessages.SERVICE_NAME_MINIMO)
                .MaximumLength(120).WithMessage(ResourceErrorMessages.SERVICE_NAME_MAXIMO);

            RuleFor(billing => billing.Amount)
                .GreaterThanOrEqualTo(0).WithMessage(ResourceErrorMessages.AMOUNT_MINIMO);

            RuleFor(billing => billing.Amount)
                .Equal(0)
                .When(billing => billing.Status == Communication.Enums.Status.Cancelado)
                .WithMessage(ResourceErrorMessages.AMOUNT_CANCELADO_INVALIDO);

            RuleFor(billing => billing.PaymentMethod)
                .IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_METHOD_INVALIDO);
            
            RuleFor(billing => billing.Status)
                .IsInEnum().WithMessage(ResourceErrorMessages.STATUS_INVALIDO);

            RuleFor(billing => billing.Notes)
                .MaximumLength(500).WithMessage(ResourceErrorMessages.NOTES_MAXIMO);
        }
    }
}
