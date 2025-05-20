namespace Front.Models.Requests;

public class CreateNoteRequest
{
    public required int PatientId { get; init; }
    public required string Note { get; init; }
}
