using ProductAPI.DTOs.GetSalesReport;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ProductAPI.Feature.Reports.Pdf;

public class SalesReportDocument : IDocument
{
    private readonly SalesReportDto _report;

    public SalesReportDocument(SalesReportDto report)
    {
        _report = report;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(35);
            page.DefaultTextStyle(style => style.FontSize(10));

            page.Header().Column(column =>
            {
                column.Item().Text("Sales Report")
                    .FontSize(20)
                    .SemiBold();

                column.Item().Text(
                    $"Period: {_report.From:dd MMM yyyy} - {_report.To:dd MMM yyyy}")
                    .FontColor(Colors.Grey.Darken1);
            });

            page.Content().PaddingVertical(20).Column(column =>
            {
                column.Spacing(15);

                column.Item().Row(row =>
                {
                    row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten1)
                        .Padding(10)
                        .Column(card =>
                        {
                            card.Item().Text("Total Orders")
                                .FontColor(Colors.Grey.Darken1);

                            card.Item().Text(_report.TotalOrders.ToString())
                                .FontSize(16)
                                .SemiBold();
                        });

                    row.ConstantItem(12);

                    row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten1)
                        .Padding(10)
                        .Column(card =>
                        {
                            card.Item().Text("Total Revenue")
                                .FontColor(Colors.Grey.Darken1);

                            card.Item().Text($"{_report.TotalRevenue:N2} EGP")
                                .FontSize(16)
                                .SemiBold();
                        });

                    row.ConstantItem(12);

                    row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten1)
                        .Padding(10)
                        .Column(card =>
                        {
                            card.Item().Text("Average Order Value")
                                .FontColor(Colors.Grey.Darken1);

                            card.Item().Text($"{_report.AverageOrderValue:N2} EGP")
                                .FontSize(16)
                                .SemiBold();
                        });
                });

                column.Item().Text("Orders")
                    .FontSize(14)
                    .SemiBold();

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(55);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderStyle).Text("Order #");
                        header.Cell().Element(HeaderStyle).Text("Date");
                        header.Cell().Element(HeaderStyle).Text("Customer");
                        header.Cell().Element(HeaderStyle).AlignRight().Text("Total");
                    });

                    foreach (var order in _report.Orders)
                    {
                        table.Cell().Element(CellStyle).Text($"#{order.OrderId}");
                        table.Cell().Element(CellStyle).Text(order.OrderDate.ToString("dd MMM yyyy"));
                        table.Cell().Element(CellStyle).Text(order.CustomerName);
                        table.Cell().Element(CellStyle).AlignRight()
                            .Text($"{order.TotalAmount:N2} EGP");
                    }
                });
            });

            page.Footer().AlignCenter().Text(text =>
            {
                text.Span("Generated on ");
                text.Span(DateTime.UtcNow.ToString("dd MMM yyyy HH:mm UTC"));

                text.Span("  •  Page ");
                text.CurrentPageNumber();
                text.Span(" of ");
                text.TotalPages();
            });
        });
    }

    private static IContainer HeaderStyle(IContainer container)
    {
        return container
            .Background(Colors.Grey.Darken2)
            .PaddingVertical(7)
            .PaddingHorizontal(5)
            .DefaultTextStyle(x => x.FontColor(Colors.White).SemiBold());
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(6)
            .PaddingHorizontal(5);
    }
}