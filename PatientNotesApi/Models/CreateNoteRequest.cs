namespace PatientNotesApi.Models;

public class CreateNoteRequest
{
    public required int PatientId { get; set; }
    public required string Note { get; set; }
}
