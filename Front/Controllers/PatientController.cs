using Back.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

public class PatientController : Controller
{
    private readonly HttpClient httpClient;

    public PatientController(IHttpClientFactory clientFactory)
    {
        httpClient = clientFactory.CreateClient("api");
    }

    [HttpGet("patients")]
    public async Task<IActionResult> Index()
    {
        var patients = await httpClient.GetFromJsonAsync<List<Patient>>("patient");
        if (patients != null)
        {
            return View(patients);
        }
        
        return View(Array.Empty<Patient>());
    }

    [HttpGet("patients/{id}")]
    public async Task<IActionResult> Infos(int id)
    {
        var patient = await httpClient.GetFromJsonAsync<Patient>($"patient/{id}");
        if (patient != null)
        {
            return View(patient);
        }

        return NotFound();
    }
}
