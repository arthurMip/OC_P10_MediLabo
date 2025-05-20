using Front.Models.Enums;
using Front.Models.Responses;

public class PatientInfoResponse
{
    public required int Id { get; init; }
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required Gender Gender { get; init; }
    public string? PostalAddress { get; init; }
    public string? PhoneNumber { get; init; }
    public required DiabetesRisk DiabetesRisk { get; init; }
    public required NoteResponse[] Notes { get; init; }
}
