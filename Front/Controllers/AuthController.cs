﻿using Front.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }
}
