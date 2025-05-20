using DiabeteReportApi.Models.Enums;

namespace DiabeteReportApi.Models.Requests;

public class DiabetesReportResponse
{
    public DiabetesRisk Result { get; set; }

    public DiabetesReportResponse(DiabetesRisk risk)
    {
        Result = risk;
    }
}
