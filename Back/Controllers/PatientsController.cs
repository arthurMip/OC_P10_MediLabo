using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Mapping;
using PatientApi.Models.Requests;
using PatientApi.Services;

namespace PatientApi.Controllers;

[Authorize]
[Route("api/patients")]
[ApiController]
public class PatientsController(PatientService patientService) : ControllerBase
{
    private readonly PatientService patientService = patientService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var patients = await patientService.GetAllAsync();
            var response = patients.MapToResponse();
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var patient = await patientService.GetByIdAsync(id);


            if (patient is null)
            {
                return NotFound();
            }


            return Ok(patient.ToResponse());
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = request.ToPatient();

            var result = await patientService.CreateAsync(patient);
            if (result == false)
            {
                return StatusCode(500);
            }

            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePatientRequest model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = model.MapToPatient(id);
            var result = await patientService.UpdateAsync(patient);
            if (result is null)
            {
                return NotFound();
            }

            var response = result.MapToResponse();
            return Ok(response);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            if (await patientService.ExistAsync(id) == false)
            {
                return NotFound();
            }

            bool deleted = await patientService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
