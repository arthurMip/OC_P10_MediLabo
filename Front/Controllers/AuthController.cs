using Front.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory clientFactory;
    public AuthController(IHttpClientFactory _clientFactory)
    {
        clientFactory = _clientFactory;
    }


    [HttpGet("/login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("/login")]

    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var httpClient = clientFactory.CreateClient("auth_api");

        var response = await httpClient.PostAsJsonAsync("auth/login", model);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();


            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
}
