using MongoDB.Bson;
using PatientNotesApi.Models.Responses;

namespace PatientNotesApi.Database;

public class PatientNote
{
    public ObjectId Id { get; set; }
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public NoteResponse ToResponse()
    {
        return new NoteResponse
        {
            Note = Note,
            CreatedAt = CreatedAt
        };
    }
}
