using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Front.ViewModels;
using Front.Mapping;
using Front.Models.Enums;
using Front.Models.Requests;
using Front.Models.Responses;

namespace Front.Controllers;

//[Authorize]
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
            //var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
            //patientsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

            //var patients = await patientsApi.GetFromJsonAsync<PatientResponse[]>("patients");

            var client = new HttpClient();

            var patients = await client.GetFromJsonAsync<PatientResponse[]>("http://gateway:8080/api/patients");

            Console.WriteLine($"Patietns: {patients?.Length}");


            if (patients is not null)
            {
                return View(patients);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return View(Array.Empty<PatientResponse>());
    }


    [HttpGet("/patients/{id}")]
    public async Task<IActionResult> Infos(int id)
    {
        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        patientsApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        notesApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await patientsApi.GetFromJsonAsync<PatientInfoResponse>($"patients/{id}");
        if (response is null)
        {
            return NotFound();
        }

        return View(response);
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
