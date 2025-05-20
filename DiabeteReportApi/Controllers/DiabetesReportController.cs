using DiabeteReportApi.Models.Requests;
using DiabeteReportApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

 namespace DiabeteReportApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DiabetesReportController : ControllerBase
{
    public DiabetesReportController() {}

    [HttpGet("/diabetes-report")]
    public IActionResult GetReport([FromBody] DiabetesReportRequest request)
    {
        var result = DiabetesReportService.GenerateReport(request);

        return Ok(result);
    }
}
