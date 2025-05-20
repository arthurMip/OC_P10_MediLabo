using Microsoft.AspNetCore.Mvc;
using PatientNotesApi.Models.Requests;
using PatientNotesApi.Services;

namespace PatientNotesApi.Controllers;

[Route("api/notes")]
[ApiController]
public class PatientNotesController(PatientNotesService patientNotesService) : ControllerBase
{
    private readonly PatientNotesService _patientNotesService = patientNotesService;

    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetNotesByPatientId([FromRoute] int patientId)
    {
        try
        {
            var notes = await _patientNotesService.GetNotesByPatientIdAsync(patientId);
            return Ok(notes);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
    {
        try
        {
            var note = request.ToPatientNote();

            await _patientNotesService.CreateNoteAsync(note);

            return Ok();

        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
