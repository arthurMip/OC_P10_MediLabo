using System.Net.Http.Headers;
using Front.Models.Enums;
using Front.Models.Requests;
using Front.Models.Responses;
using Front.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

[Authorize]
public class PatientController(IHttpClientFactory clientFactory) : Controller
{
    private readonly HttpClient client = clientFactory.CreateClient("gateway");

    [HttpGet("/patients")]
    public async Task<IActionResult> Index()
    {
        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    
        var patients = await client.GetFromJsonAsync<PatientResponse[]>("patients");
        if (patients is null)
        {
            return View(Array.Empty<PatientResponse>());
        }
        return View(patients);
    }


    [HttpGet("/patients/{id}")]
    public async Task<IActionResult> Infos(int id)
    {
        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await client.GetFromJsonAsync<PatientInfoResponse>($"patients/{id}");
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
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await client.PostAsJsonAsync("patients", request);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Une erreur est survenue lors de la création du patient.");
            return View(patient);
        }
        return RedirectToAction("Index");
    }

    [HttpGet("/patients/update/{id}")]
    public async Task<IActionResult> Update(int id)
    {
        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await client.GetFromJsonAsync<PatientInfoResponse>($"patients/{id}");
        if (response is null)
        {
            return NotFound();
        }

        var model = new UpdatePatientViewModel
        {
            Id = response.Id,
            Firstname = response.Firstname,
            Lastname = response.Lastname,
            BirthDate = response.BirthDate.ToDateTime(new TimeOnly()),
            Gender = response.Gender == Gender.Male ? "M" : "F",
            PostalAddress = response.PostalAddress,
            PhoneNumber = response.PhoneNumber,
        };

        return View(model);
    }

    [HttpPost("/patients/update/{id}")]
    public async Task<IActionResult> Update(int id, UpdatePatientViewModel patient)
    {
        if (!ModelState.IsValid)
        {
            return View(patient);
        }

        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new UpdatePatientRequest
        {
            Firstname = patient.Firstname.Trim(),
            Lastname = patient.Lastname.Trim(),
            BirthDate = DateOnly.FromDateTime(patient.BirthDate),
            Gender = patient.Gender == "M" ? Gender.Male : Gender.Female,
            PostalAddress = patient.PostalAddress?.Trim(),
            PhoneNumber = patient.PhoneNumber?.Trim()
        };

        var response = await client.PutAsJsonAsync($"patients/{id}", request); 
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la mise à jour du patient.");
            return View(patient);
        }
        
        return RedirectToAction("Infos", new { id });
    }
}
