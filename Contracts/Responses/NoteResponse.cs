namespace Contracts.Responses;

public class NoteResponse
{
    public required int PatientId { get; init; }
    public required string Note { get; init; }
    public required DateTime CreatedAt { get; init; }
}
