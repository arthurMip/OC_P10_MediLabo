using DiabeteReportApi.Models.Enums;

namespace DiabeteReportApi.Models.Requests;

public class DiabetesReportRequest
{
    public required int Age { get; init; }
    public required Gender Gender { get; set; }
    public required string Notes { get; init; }
}
