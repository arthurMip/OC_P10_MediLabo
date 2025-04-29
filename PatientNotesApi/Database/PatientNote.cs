using MongoDB.Bson;

namespace PatientNotesApi.Database;

public class PatientNote
{
    public ObjectId Id { get; set; }
    public int PatientId { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
