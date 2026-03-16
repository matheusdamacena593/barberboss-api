using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions
{
    public static class PaymentTypeExtensions
    {
        public static string PaymentTypeToString(this PaymentMethod paymentType)
        {
            return paymentType switch
            {
                PaymentMethod.CartaoCredito => ResourceReportGenerationMessages.CARTAO_CREDITO,
                PaymentMethod.CartaoDebito => ResourceReportGenerationMessages.CARTAO_DEBITO,
                PaymentMethod.Dinheiro => ResourceReportGenerationMessages.DINHEIRO,
                PaymentMethod.Pix => ResourceReportGenerationMessages.PIX,
                PaymentMethod.Outro => ResourceReportGenerationMessages.OUTRO,
                _ => string.Empty
            };
        }
    }
}
