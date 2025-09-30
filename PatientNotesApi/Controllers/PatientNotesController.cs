using Microsoft.AspNetCore.Mvc;
using PatientNotesApi.Models.Requests;
using PatientNotesApi.Services;

namespace PatientNotesApi.Controllers;

[Route("api/notes")]
[ApiController]
public class PatientNotesController(PatientNotesService patientNotesService) : ControllerBase
{
    private readonly PatientNotesService _patientNotesService = patientNotesService;

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var allNotes = await _patientNotesService.GetAllAsync();

        return Ok($"Patient Notes API is working!, total notes in db: {allNotes.Count}");
    }

    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetNotesByPatientId([FromRoute] int patientId)
    {
        var notes = await _patientNotesService.GetNotesByPatientIdAsync(patientId);
        return Ok(notes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
    {
        var note = request.ToPatientNote();
        await _patientNotesService.CreateNoteAsync(note);
        return Ok();
    }
}
