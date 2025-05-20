using DiabeteReportApi.Models.Enums;
using DiabeteReportApi.Models.Requests;

namespace DiabeteReportApi.Services;

public static class DiabetesReportService
{
    public static readonly string[] keywords = [
        "Hémoglobine A1C",
        "Microalbumine",
        "Taille",
        "Poids",
        "Fumeur",
        "Fumeuse",
        "Anormal",
        "Cholestérol",
        "Vertiges",
        "Rechute",
        "Réaction",
        "Anticorps"
    ];

    public static DiabetesReportResponse GenerateReport(DiabetesReportRequest request)
    {
        var age = request.Age;
        var gender = request.Gender;
        var words = 0;

        foreach (string keyword in keywords)
        {
            if (request.Notes.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                words++;
            }
        }

        if (words == 0)
        {
            return new DiabetesReportResponse(DiabetesRisk.None);
        }

        if (words >= 2 && words <= 5 && age > 30)
        {
            return new DiabetesReportResponse(DiabetesRisk.Borderline);
        }

        if (gender == Gender.Male && age < 30 && words == 3)
        {
            return new DiabetesReportResponse(DiabetesRisk.InDanger);
        }

        if (gender == Gender.Female && age < 30 && words == 4)
        {
            return new DiabetesReportResponse(DiabetesRisk.InDanger);
        }

        if (age > 30 && (words == 6 || words == 7))
        {
            return new DiabetesReportResponse(DiabetesRisk.InDanger);
        }

        if (gender == Gender.Male && age < 30 && words >= 5)
        {
            return new DiabetesReportResponse(DiabetesRisk.EarlyOnset);
        }

        if (gender == Gender.Female && age < 30 && words >= 7)
        {
            return new DiabetesReportResponse(DiabetesRisk.EarlyOnset);
        }

        if (age > 30 && words >= 8)
        {
            return new DiabetesReportResponse(DiabetesRisk.EarlyOnset);
        }

        return new DiabetesReportResponse(DiabetesRisk.None);
    }
}
