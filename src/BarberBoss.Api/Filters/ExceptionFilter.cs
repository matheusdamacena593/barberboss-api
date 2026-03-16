using BarberBoss.Communication.Responses;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberBoss.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BarberBossException)
            {
                HandleProjectException(context);
            }
            else
            {
                ThrowUnknowError(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            var barberBossException = (BarberBossException)context.Exception;
            var erroResponse = new ResponseErrorsJson(barberBossException.GetErrors());

            context.HttpContext.Response.StatusCode = barberBossException.StatusCode;
            context.Result = new ObjectResult(erroResponse);
        }

        private void ThrowUnknowError(ExceptionContext context)
        {
            var erroResponse = new ResponseErrorsJson(ResourceErrorMessages.ERRO_DESCONHECIDO);

            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(erroResponse);
        }
    }
}
