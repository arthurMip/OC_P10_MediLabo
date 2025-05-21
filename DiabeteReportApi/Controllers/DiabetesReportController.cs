using DiabeteReportApi.Models.Requests;
using DiabeteReportApi.Services;
using Microsoft.AspNetCore.Mvc;

 namespace DiabeteReportApi.Controllers;

[Route("api/diabetes-reports")]
[ApiController]
public class DiabetesReportController : ControllerBase
{
    public DiabetesReportController() {}

    [HttpPost]
    public IActionResult GetReport([FromBody] DiabetesReportRequest request)
    {
        var result = DiabetesReportService.GenerateReport(request);

        return Ok(result);
    }
}
