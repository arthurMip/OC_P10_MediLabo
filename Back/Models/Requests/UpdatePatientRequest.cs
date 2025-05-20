using PatientApi.Data.Entities;
using PatientApi.Models.Enums;

namespace PatientApi.Models.Requests;

public class UpdatePatientRequest
{
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required Gender Gender { get; init; }
    public string? PostalAddress { get; init; }
    public string? PhoneNumber { get; init; }

    public Patient ToPatient()
    {
        return new Patient
        {
            Firstname = Firstname,
            Lastname = Lastname,
            BirthDate = BirthDate,
            Gender = Gender,
            PostalAddress = PostalAddress ?? string.Empty,
            PhoneNumber = PhoneNumber ?? string.Empty,
        };
    }
}
