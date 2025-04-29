using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Models;
using PatientApi.Services;

namespace PatientApi.Controllers;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PatientController(PatientService patientService) : ControllerBase
{
    private readonly PatientService patientService = patientService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var patients = await patientService.GetAllAsync();

            return Ok(patients);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var patient = await patientService.GetByIdAsync(id);
            if (patient != null)
            {
                return Ok(patient);
            }
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePatientRequest model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patient = await patientService.CreateAsync(model);
            if (patient != null)
            {
                return Ok(patient);
            }

            return StatusCode(500);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePatientRequest model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await patientService.ExistAsync(id) == false)
            {
                return NotFound();
            }


            var patient = await patientService.UpdateAsync(id, model);
            
            if (patient != null)
            {
                return Ok(patient);
            }

            return StatusCode(500);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (await patientService.ExistAsync(id) == false)
            {
                return NotFound();
            }

            var result = await patientService.DeleteAsync(id);
            if (result == true)
            {
                return NoContent();
            }

            return StatusCode(500);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
