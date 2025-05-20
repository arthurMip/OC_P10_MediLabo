using PatientApi.Models.Enums;

namespace PatientApi.Models.Responses;

public class PatientResponse
{
    public required int Id { get; init; }
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required Gender Gender { get; init; }
    public string? PostalAddress { get; init; }
    public string? PhoneNumber { get; init; }
}
