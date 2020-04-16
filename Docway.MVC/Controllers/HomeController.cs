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

        //[HttpGet("medicos")]
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
                dados = await response.Content.ReadAsStringAsync();
                medicos = JsonConvert.DeserializeObject<List<Medicos>>(dados);
                isAuthorized = true;
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
            //getTokenByRefreshToken();
            //getToken();
            return View();
        }

        public async void getToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:5000/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            /*
            var parametro = new
            {
                client_id = "mvc1",
                client_secret = "segredo",
                grant_type = "refresh_token",
                refresh_token = refreshToken
            };*/

            var KeyValues = new List<KeyValuePair<string,string>>();
            KeyValues.Add(new KeyValuePair<string, string>("client_id","mvc1"));
            KeyValues.Add(new KeyValuePair<string, string>("client_secret","segredo"));
            KeyValues.Add(new KeyValuePair<string, string>("grant_type","refresh_token"));
            KeyValues.Add(new KeyValuePair<string, string>("refresh_token",refreshToken));

            //var jsonContent = JsonConvert.SerializeObject(parametro); 
            //var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/x-www-form-urlencoded");

            var contentString = new FormUrlEncodedContent(KeyValues);
            
            contentString.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded"); 
            //contentString.Headers.Add("Session-Token", session_token); 

            //Console.WriteLine(parametro);

            HttpResponseMessage response = await client.PostAsync("connect/token", contentString);

            Console.WriteLine(response);

            var dados = string.Empty;
            var token = new Token();
            
            if(response.IsSuccessStatusCode)
            {
                dados = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<Token>(dados);
            }

            Console.WriteLine(token.Access_token);
            
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
