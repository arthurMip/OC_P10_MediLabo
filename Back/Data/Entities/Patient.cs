using PatientApi.Models.Enums;
using PatientApi.Models.Responses;

namespace PatientApi.Data.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string PostalAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;


    public PatientResponse ToResponse()
    {
        return new PatientResponse
        {
            Id = Id,
            Firstname = Firstname,
            Lastname = Lastname,
            BirthDate = BirthDate,
            Gender = Gender,
            PostalAddress = string.IsNullOrEmpty(PostalAddress) ? null : PostalAddress,
            PhoneNumber = string.IsNullOrEmpty(PhoneNumber) ? null : PhoneNumber,
        };
    }

}

