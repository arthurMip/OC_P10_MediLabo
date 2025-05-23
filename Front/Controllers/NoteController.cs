﻿using System.Net.Http.Headers;
using Front.Models.Requests;
using Front.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

[Authorize]
public class NoteController : Controller
{
    private readonly HttpClient patientsApi;
    private readonly HttpClient notesApi;

    public NoteController(IHttpClientFactory clientFactory)
    {
        patientsApi = clientFactory.CreateClient("patients_api");
        notesApi = clientFactory.CreateClient("notes_api");
    }


    [HttpGet("/notes/{patientId}/create")]
    public IActionResult Create([FromRoute] int patientId)
    {
        var viewModel = new CreateNoteViewModel
        {
            PatientId = patientId
        };
        return View(viewModel);
    }

    [HttpPost("/notes/{patientId}/create")]
    public async Task<IActionResult> Create([FromRoute] int patientId, [FromForm] CreateNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var jwt = Request.Cookies.FirstOrDefault(c => c.Key == "jwt").Value;
        notesApi.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var request = new CreateNoteRequest
        {
            PatientId = patientId,
            Note = model.Note
        };

        var response = await notesApi.PostAsJsonAsync($"notes", request);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Infos", "Patient", new { id = patientId });
        }
        ModelState.AddModelError(string.Empty, "An error occurred while creating the note.");
        return View(model);
    }
}
