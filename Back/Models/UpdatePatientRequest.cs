using Back.Enums;
using System.ComponentModel.DataAnnotations;

namespace Back.Models;

public class UpdatePatientRequest
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public DateOnly? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? PostalAddress { get; set; }
    public string? PhoneNumber { get; set; }
}
