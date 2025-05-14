using PatientApi.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Contracts.Requests;
using Contracts.Enums;
using Contracts.Responses;
using Azure;
using Front.ViewModels;
using Front.Mapping;

namespace Front.Controllers;

[Authorize]
public class PatientController : Controller
{
    private readonly HttpClient patientsApi;
    private readonly HttpClient notesApi;


    public PatientController(IHttpClientFactory clientFactory)
    {
        patientsApi = clientFactory.CreateClient("patients_api");
        notesApi = clientFactory.CreateClient("notes_api");
    }

    [HttpGet("/patients")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
            patientsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            var patients = await patientsApi.GetFromJsonAsync<IEnumerable<PatientResponse>>("patients");

            if (patients is not null)
            {
                return View(patients.MapToViewModel());
            }
        }
        catch (HttpRequestException)
        {

        }
        catch (Exception)
        {
        }

        return View(Array.Empty<PatientListItemViewModel>());
    }


    [HttpGet("/patients/{id}")]
    public async Task<IActionResult> Infos(int id)
    {
        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        patientsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        notesApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var patient = await patientsApi.GetFromJsonAsync<PatientResponse>($"patients/{id}");
        var notes = await notesApi.GetFromJsonAsync<IEnumerable<NoteResponse>>($"notes/{id}");

        if (patient is not null && notes is not null)
        {
            return View(patient.MapToViewModel(notes));
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

        var request = new CreatePatientRequest
        {
            Firstname = patient.Firstname.Trim(),
            Lastname = patient.Lastname.Trim(),
            BirthDate = DateOnly.FromDateTime(patient.BirthDate),
            Gender = patient.Gender == "M" ? Gender.Male : Gender.Female,
            PostalAddress = patient.PostalAddress?.Trim(),
            PhoneNumber = patient.PhoneNumber?.Trim()
        };

        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        patientsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);


        var response = await patientsApi.PostAsJsonAsync("patients", request);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création du patient.");
        return View(patient);
    }
}
