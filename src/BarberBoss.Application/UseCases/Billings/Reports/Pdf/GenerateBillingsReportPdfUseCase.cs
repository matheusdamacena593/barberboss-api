using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Billings.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Billings;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Billings.Reports.Pdf
{
    public class GenerateBillingsReportPdfUseCase : IGenerateBillingsReportPdfUseCase
    {
        private const string CURRENCY_SYMBOL = "R$";
        private const int HEIGHT_ROW_BILLING_TABLE = 25;
        private readonly IBillingsReadOnlyRepository _repository;

        public GenerateBillingsReportPdfUseCase(IBillingsReadOnlyRepository repository)
        {
            _repository = repository;

            GlobalFontSettings.FontResolver = new BillingsReportFontResolver();
        }

        public async Task<byte[]> Execute(DateOnly date)
        {
            var billings = await _repository.FilterByWeek(date);

            if (billings.Count == 0)
            {
                return [];
            }

            var document = CreateDocument(date);
            var page = CreatePage(document);

            CreateHeaderWithProfilePhotoAndName(page);

            var totalBillings = billings.Sum(billing => billing.Amount);
            CreateTotalSpentSection(page, date, totalBillings);

            foreach (var billing in billings)
            {
                var table = CreateBillingTable(page);

                var row = table.AddRow();
                row.Height = HEIGHT_ROW_BILLING_TABLE;

                AddBillingServiceName(row.Cells[0], billing.ServiceName);

                AddHeaderForAmount(row.Cells[3]);

                row = table.AddRow();
                row.Height = HEIGHT_ROW_BILLING_TABLE;

                row.Cells[0].AddParagraph(billing.Date.ToString("D"));
                SetStyleBaseForBillingInformation(row.Cells[0]);
                row.Cells[0].Format.LeftIndent = 20;

                row.Cells[1].AddParagraph(billing.Date.ToString("t"));
                SetStyleBaseForBillingInformation(row.Cells[1]);

                row.Cells[2].AddParagraph(billing.PaymentMethod.PaymentTypeToString());
                SetStyleBaseForBillingInformation(row.Cells[2]);

                AddAmountForBilling(row.Cells[3], billing.Amount);

                if (!string.IsNullOrWhiteSpace(billing.Notes))
                {
                    var description = table.AddRow();
                    description.Height = HEIGHT_ROW_BILLING_TABLE;
                    
                    AddDescriptionForBilling(description.Cells[0], billing.Notes);

                    row.Cells[3].MergeDown = 1;
                }

                AddWhiteSpaces(table);
            }

            return RenderDocument(document);
        }

        private Document CreateDocument(DateOnly month)
        {
            var document = new Document();
            document.Info.Title = $"{ResourceReportGenerationMessages.RECEITA_PARA} {month:Y}";
            document.Info.Author = "Matheus Damacena";

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.RALEWAY_REGULAR;

            return document;
        }

        private Section CreatePage(Document document)
        {
            var section = document.AddSection();
            section.PageSetup = document.DefaultPageSetup.Clone();

            section.PageSetup.PageFormat = PageFormat.A4;

            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;
            section.PageSetup.TopMargin = 80;
            section.PageSetup.BottomMargin = 80;

            return section;
        }

        private void CreateHeaderWithProfilePhotoAndName(Section page)
        {
            var table = page.AddTable();
            table.AddColumn();
            table.AddColumn("300");

            var row = table.AddRow();

            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);

            var pathFile = Path.Combine(directoryName!, "Logo", "logo-md.png");

            row.Cells[0].AddImage(pathFile);

            row.Cells[1].AddParagraph("Barbearia do Matheus");
            row.Cells[1].Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        }

        private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalBillings)
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "40";
            paragraph.Format.SpaceAfter = "40";

            var title = string.Format(ResourceReportGenerationMessages.FATURAMENTO_DA_SEMANA);

            paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_BLACK, Size = 15 });

            paragraph.AddLineBreak();

            paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalBillings}", new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 50 });
        }

        private Table CreateBillingTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

            return table;
        }

        private void AddBillingServiceName(Cell cell, string billingServiceName)
        {
            cell.AddParagraph(billingServiceName);
            cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.GREEN_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 20;
        }

        private void AddHeaderForAmount(Cell cell)
        {
            cell.AddParagraph(ResourceReportGenerationMessages.VALOR);
            cell.Format.Font = new Font { Name = FontHelper.RALEWAY_BLACK, Size = 14, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.GREEN_LIGTH;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Format.RightIndent = 5;
        }

        private void SetStyleBaseForBillingInformation(Cell cell)
        {
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.GRAY_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private void AddAmountForBilling(Cell cell, decimal billingAmount)
        {
            cell.AddParagraph($"{CURRENCY_SYMBOL} {billingAmount}");
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.WHITE;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private void AddWhiteSpaces(Table table)
        {
            var row = table.AddRow();
            row.Height = 30;
            row.Borders.Visible = false;
        }

        private void AddDescriptionForBilling(Cell cell, string billingDescription)
        {
            cell.AddParagraph(billingDescription);
            cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.GRAY_LIGTH;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 20;
        }

        private byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer
            {
                Document = document,
            };

            renderer.RenderDocument();

            using var file = new MemoryStream();
            renderer.PdfDocument.Save(file);

            return file.ToArray();
        }
    }
}
