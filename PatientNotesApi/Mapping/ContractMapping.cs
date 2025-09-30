using PatientNotesApi.Database;
using PatientNotesApi.Models.Requests;

namespace PatientNotesApi.Mapping;

public static class ContractMapping
{
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
