namespace PatientApi.Models.Responses;

public class NoteResponse
{
    public required string Note { get; init; }
    public required DateTime CreatedAt { get; init; }
}
