using Contracts.Enums;

namespace Front.ViewModels;

public class PatientDetailViewModel
{
    public required int Id { get; init; }
    public required string Firstname { get; init; }
    public required string Lastname { get; init; }
    public required Gender Gender { get; init; }
    public required DateOnly BirthDate { get; init; }
    public string? PostalAddress { get; init; }
    public string? PhoneNumber { get; init; }
    public IEnumerable<NoteViewModel> Notes { get; init; } = [];
}
