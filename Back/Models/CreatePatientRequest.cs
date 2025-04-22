using System.ComponentModel.DataAnnotations;
using PatientApi.Enums;

namespace PatientApi.Models;

public class CreatePatientRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Firstname { get; set; } = string.Empty;
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Lastname { get; set; } = string.Empty;
    [Required]
    public DateOnly? BirthDate { get; set; }
    [Required]
    public Gender? Gender { get; set; }
    public string? PostalAddress { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
}
