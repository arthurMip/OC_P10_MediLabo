using System.Diagnostics;
using Back.Data.Entities;
using Front.Models;
using Microsoft.AspNetCore.Mvc;

namespace Front.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;

        public HomeController(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("api");
        }

        public async Task<IActionResult> Index()
        {
            var patients = await httpClient.GetFromJsonAsync<List<Patient>>("patient");
            if (patients != null)
            {
                return View(patients);
            }
            
            return View(Array.Empty<Patient>());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
