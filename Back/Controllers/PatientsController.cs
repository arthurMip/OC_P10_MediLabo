using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientApi.Mapping;
using PatientApi.Models.Enums;
using PatientApi.Models.Requests;
using PatientApi.Models.Responses;
using PatientApi.Services;

namespace PatientApi.Controllers;

[Authorize]
[Route("api/patients")]
[ApiController]
public class PatientsController(PatientService patientService, IHttpClientFactory clientFactory) : ControllerBase
{
    private readonly HttpClient _client = clientFactory.CreateClient("gateway");

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var patients = await patientService.GetAllAsync();
        var response = patients.MapToResponse();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var patient = await patientService.GetByIdAsync(id);
        if (patient is null)
        {
            return NotFound();
        }

        var notesResponse = await _client.GetFromJsonAsync<NoteResponse[]>($"notes/{id}");

        var notes = notesResponse ?? [];

        var diabeteRisk = DiabetesRisk.None;

        if (notes.Length > 0)
        {
            var reportRequest = new DiabetesReportRequest {
                Age = CalculateAge(patient.BirthDate),
                Gender = patient.Gender,
                Notes = notes.Select(n => n.Note).ToArray()
            };
            var reportResponse = await _client.PostAsJsonAsync("diabetes-reports", reportRequest);

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var patient = request.ToPatient();
        var result = await patientService.CreateAsync(patient);
        if (!result)
        {
            return StatusCode(500);
        }

        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePatientRequest model)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var exist = await patientService.ExistAsync(id);
        if (!exist)
        {
            return NotFound();
        }

        bool deleted = await patientService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
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
