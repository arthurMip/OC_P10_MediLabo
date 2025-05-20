namespace Front.ViewModels;

public class CreateNoteViewModel
{
    public required int PatientId  { get; init; }
    public string Note { get; set; } = string.Empty;
}
