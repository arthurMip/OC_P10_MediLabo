using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Front.ViewModels;

public class UpdatePatientViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [DisplayName("Nom de famille")]
    public string Firstname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [DisplayName("Prénom")]
    public string Lastname { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Date de naissance")]
    public DateTime BirthDate { get; set; }

    [Required]
    [DisplayName("Sexe")]
    public string Gender { get; set; } = "M";

    [DisplayName("Adresse")]
    public string? PostalAddress { get; set; }

    [DataType(DataType.PhoneNumber)]
    [DisplayName("Numéro de téléphone")]
    public string? PhoneNumber { get; set; }
}
