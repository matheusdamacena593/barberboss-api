using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Services.LoggedUser;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Billings.Reports.Excel
{
    public class GenerateBillingsReportExcelUseCase : IGenerateBillingsReportExcelUseCase
    {
        private const string CURRENCY_SYMBOL = "R$";
        private readonly IBillingsReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public GenerateBillingsReportExcelUseCase(
            IBillingsReadOnlyRepository repository,
            ILoggedUser loggedUser)
        {
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task<byte[]> Execute(DateOnly date)
        {
            var loggedUser = await _loggedUser.Get();

            var expenses = await _repository.FilterByWeek(date, loggedUser);

            if (expenses.Count == 0)
            {
                return [];
            }

            using var workbook = new XLWorkbook();

            workbook.Author = loggedUser.Name;
            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(date.ToString("Y"));

            InsertHeader(worksheet);

            var raw = 2;
            foreach (var expense in expenses)
            {
                worksheet.Cell($"A{raw}").Value = expense.ServiceName;
                worksheet.Cell($"B{raw}").Value = expense.Date;
                worksheet.Cell($"C{raw}").Value = expense.PaymentMethod.PaymentTypeToString();

                worksheet.Cell($"D{raw}").Value = expense.Amount;
                worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00";

                worksheet.Cell($"E{raw}").Value = expense.Notes;

                raw++;
            }

            worksheet.Columns().AdjustToContents();

            var file = new MemoryStream();
            workbook.SaveAs(file);

            return file.ToArray();
        }

        private void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITULO;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATA;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.TIPO_DE_PAGAMENTO;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.VALOR;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRICAO;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.White;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");
            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
    }
}
