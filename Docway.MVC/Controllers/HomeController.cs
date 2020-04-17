using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Docway.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Docway.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Token()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            
            ViewBag.Token = accessToken;
            ViewBag.Refresh_Token = refreshToken;

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("cookie_mvc_docway");
            await HttpContext.SignOutAsync("oidc");
        }

        public async Task<IActionResult> MedicosAPI()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            HttpClient cliente = new HttpClient();
            cliente.BaseAddress = new Uri("http://localhost:5002");

            cliente.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            cliente.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            
            HttpResponseMessage response = await cliente.GetAsync("medicos");
            
            var dados = string.Empty;
            var medicos = new Object();
            var isAuthorized = false;
            if(response.IsSuccessStatusCode)
            {
                isAuthorized = true;
                dados = await response.Content.ReadAsStringAsync();
                medicos = JsonConvert.DeserializeObject<List<Medicos>>(dados);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                isAuthorized = false;
            }

            if(isAuthorized)
            {
                ViewBag.medicos = medicos;
            }
            else
            {
                ViewBag.medicos = null;
            }
            
            return View();
        }

        

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
