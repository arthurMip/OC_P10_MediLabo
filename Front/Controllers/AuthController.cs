using Front.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Front.Controllers;

[AllowAnonymous]
public class AuthController(IHttpClientFactory clientFactory) : Controller
{
    private readonly IHttpClientFactory _clientFactory = clientFactory;

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

        var client = _clientFactory.CreateClient("auth_api");

        var response = await client.PostAsJsonAsync("auth/login", model);
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Tentative de connexion invalide.");
            return View(model);
        }

        var jwt = await response.Content.ReadAsStringAsync();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        var identity = new ClaimsIdentity(token.Claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync("Cookies", principal);

        Response.Cookies.Append("jwt", jwt, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        return RedirectToAction("Index", "Patient");
    }
}
