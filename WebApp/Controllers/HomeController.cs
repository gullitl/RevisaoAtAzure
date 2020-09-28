using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Home;
using WebApp.Models.Pais;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:61347");
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                QuantidadeCarros = 10,
                QuantidadeFabricantes = 100,
                QuantidadeProprietarios = 10
            };

            return View(viewModel);
        }

        private async Task<int> ObterQuantidadeDeFabricantes()
        {
            var response = await _httpClient.GetAsync("api/Pais");

            var contentString = await response.Content.ReadAsStringAsync();

            var fabricantes = JsonConvert.DeserializeObject<List<PaisView>>(contentString);

            return fabricantes.Count;
        }

        public IActionResult Sobre() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
