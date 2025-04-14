using System.ComponentModel.DataAnnotations;
using Back.Enums;

namespace Back.DTOs;

public class CreatePatientDTO
{
    [Required]
    public string Firstname { get; set; } = string.Empty;
    [Required]
    public string Lastname { get; set; } = string.Empty;
    [Required]
    public DateOnly BirthDate { get; set; }
    [Required]
    public Gender Gender { get; set; }
    public string PostalAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
