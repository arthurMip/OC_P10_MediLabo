using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            if (notes != null)
            {
                return Ok(notes);
            }
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

}
