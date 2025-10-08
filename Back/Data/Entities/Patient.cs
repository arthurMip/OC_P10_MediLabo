using PatientApi.Models.Enums;

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
}

