using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Mapping;
using PatientApi.Models.Enums;
using PatientApi.Models.Requests;
using PatientApi.Models.Responses;
using PatientApi.Services;

namespace PatientApi.Controllers;

//[Authorize]
[Route("api/patients")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly PatientService _patientService;
    private readonly HttpClient _notesApi;
    private readonly HttpClient _reportApi;


    public PatientsController(PatientService patientService, IHttpClientFactory clientFactory)
    {
        _patientService = patientService;
        _notesApi = clientFactory.CreateClient("notes_api");
        _reportApi = clientFactory.CreateClient("report_api");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var patients = await _patientService.GetAllAsync();
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
            var patient = await _patientService.GetByIdAsync(id);
            if (patient is null)
            {
                return NotFound();
            }

            var notesResponse = await _notesApi.GetFromJsonAsync<IEnumerable<NoteResponse>>($"notes/{id}");

            var notes = notesResponse?.ToArray() ?? [];


            var diabeteRisk = DiabetesRisk.None;

            if (notes.Length > 0)
            {
                var reportRequest = new DiabetesReportRequest {
                    Age = CalculateAge(patient.BirthDate),
                    Gender = patient.Gender,
                    Notes = notes.Select(n => n.Note).ToArray()
                };
                var reportResponse = await _reportApi.PostAsJsonAsync($"", reportRequest);

                if (reportResponse.IsSuccessStatusCode)
                {
                    var report = await reportResponse.Content.ReadFromJsonAsync<DiabetesReportResponse>();
                    if (report is not null)
                    {
                        diabeteRisk = report.Result;
                    }
                }
            }

            var response = new PatientInfoResponse
            {
                Id = patient.Id,
                Firstname = patient.Firstname,
                Lastname = patient.Lastname,
                Gender = patient.Gender,
                BirthDate = patient.BirthDate,
                PostalAddress = patient.PostalAddress,
                PhoneNumber = patient.PhoneNumber,
                DiabetesRisk = diabeteRisk,
                Notes = notes,
            };

            return Ok(response);
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

            var result = await _patientService.CreateAsync(patient);
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
            var result = await _patientService.UpdateAsync(patient);
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
            if (await _patientService.ExistAsync(id) == false)
            {
                return NotFound();
            }

            bool deleted = await _patientService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
    // Helper method to compute age from DateOnly birthDate
    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - birthDate.Year;

        // Adjust age if the birthday hasn't occurred yet this year
        if (birthDate > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }
}
