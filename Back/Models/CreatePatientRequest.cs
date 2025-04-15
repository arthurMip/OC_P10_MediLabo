using System.ComponentModel.DataAnnotations;
using Back.Enums;

namespace Back.Models;

public class CreatePatientRequest
{
    [Required]
    [MinLength(3)]
    public string Firstname { get; set; } = string.Empty;
    [Required]
    [MinLength(3)]
    public string Lastname { get; set; } = string.Empty;
    [Required]
    public DateOnly? BirthDate { get; set; }
    [Required]
    public Gender? Gender { get; set; }
    public string PostalAddress { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
}
