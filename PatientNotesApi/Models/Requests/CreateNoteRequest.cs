using PatientNotesApi.Database;

namespace PatientNotesApi.Models.Requests;

public class CreateNoteRequest
{
    public required int PatientId { get; init; }
    public required string Note { get; init; }

    public PatientNote ToPatientNote()
    {
        return new PatientNote
        {
            PatientId = PatientId,
            Note = Note,
            CreatedAt = DateTime.UtcNow
        };
    }
}
