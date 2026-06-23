using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Feature.Reports.GetSalesReport.Queries;
using ProductAPI.Feature.Reports.Pdf;
using QuestPDF.Fluent;

namespace ProductAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport(
        [FromQuery] DateTime from,
        [FromQuery] DateTime to)
    {
        if (from > to)
            return BadRequest("From date cannot be after To date.");

        var result = await _mediator.Send(new GetSalesReportQuery(from, to));

        return Ok(result);
    }
    [HttpGet("sales/pdf")]
    public async Task<IActionResult> DownloadSalesReportPdf(
    [FromQuery] DateTime from,
    [FromQuery] DateTime to)
    {
        if (from > to)
            return BadRequest("From date cannot be after To date.");

        var report = await _mediator.Send(
            new GetSalesReportQuery(from, to));

        var document = new SalesReportDocument(report);

        var pdfBytes = document.GeneratePdf();

        var fileName = $"sales-report-{from:yyyy-MM-dd}-to-{to:yyyy-MM-dd}.pdf";

        return File(
            pdfBytes,
            "application/pdf",
            fileName);
    }
}