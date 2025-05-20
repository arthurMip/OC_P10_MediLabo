using Front.Models.Enums;

namespace Front.Models.Requests;

public class UpdatePatientRequest
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required Gender Gender { get; init; }
    public string? PostalAddress { get; init; }
    public string? PhoneNumber { get; init; }
}
