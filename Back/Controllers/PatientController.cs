using Back.Models;
using Back.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController(PatientService patientService) : ControllerBase
{
    private readonly PatientService patientService = patientService;

    [HttpGet]
    public IActionResult GetAll()
    {
        try
        {
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        try
        {
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPost]
    public IActionResult Create(CreatePatientRequest model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdatePatientRequest model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            return Ok();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}
