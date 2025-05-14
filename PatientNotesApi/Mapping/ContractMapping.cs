using Contracts.Requests;
using Contracts.Responses;
using PatientNotesApi.Database;

namespace PatientNotesApi.Mapping;

public static class ContractMapping
{
    public static NoteResponse MapToResponse(this PatientNote note)
    {
        return new NoteResponse
        {
            PatientId = note.PatientId,
            Note = note.Note,
            CreatedAt = note.CreatedAt
        };
    }
    public static IEnumerable<NoteResponse> MapToResponse(this IEnumerable<PatientNote> notes)
    {
        return notes.Select(MapToResponse);
    }

    public static PatientNote MapToPatientNote(this CreateNoteRequest request)
    {
        return new PatientNote
        {
            PatientId = request.PatientId,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow
        };
    }
}
