using PatientApi.Data.Entities;
using PatientApi.Models;
using PatientApi.Enums;
using Front.Models;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

public class PatientController : Controller
{
    private readonly HttpClient httpClient;

    public PatientController(IHttpClientFactory clientFactory)
    {
        httpClient = clientFactory.CreateClient("patient_api");
    }

    [HttpGet("/patients")]
    public async Task<IActionResult> Index()
    {
        var patients = await httpClient.GetFromJsonAsync<List<Patient>>("patient");
        if (patients != null)
        {
            return View(patients);
        }

        return View(Array.Empty<Patient>());
    }

    [HttpGet("/patients/{id}")]
    public async Task<IActionResult> Infos(int id)
    {
        var patient = await httpClient.GetFromJsonAsync<Patient>($"patient/{id}");
        if (patient != null)
        {
            return View(patient);
        }

        return NotFound();
    }

    [HttpGet("/patients/create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("/patients/create")]
    public async Task<IActionResult> Create(CreatePatientViewModel patient)
    {
        if (!ModelState.IsValid)
        {
            return View(patient);
        }

        var patientRequest = new CreatePatientRequest
        {
            Firstname = patient.Firstname.Trim(),
            Lastname = patient.Lastname.Trim(),
            BirthDate = DateOnly.FromDateTime(patient.BirthDate),
            Gender = patient.Gender == "M" ? Gender.Male : Gender.Female,

            PostalAddress = patient.PostalAddress?.Trim(),
            PhoneNumber = patient.PhoneNumber?.Trim()
        };

        var response = await httpClient.PostAsJsonAsync("patient", patientRequest);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création du patient.");
        return View(patient);
    }
}
