using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientNotesApi.Database;
using PatientNotesApi.Models;
using PatientNotesApi.Services;

namespace PatientNotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientNotesController : ControllerBase
{
    private readonly PatientNotesService _patientNotesService;
    public PatientNotesController(PatientNotesService patientNotesService)
    {
        _patientNotesService = patientNotesService;
    }


    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetNotesByPatientId(int patientId)
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
            var note = new PatientNote
            {
                PatientId = request.PatientId,
                Note = request.Note,
                CreatedAt = DateTime.UtcNow
            };

            await _patientNotesService.CreateNoteAsync(note);

            return Ok();

        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
