using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Models.Fabricante;
using WebApp.Models.Home;

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
            var viewModel = new HomeIndexViewModel();

            viewModel.QuantidadeCarros = 10;
            viewModel.QuantidadeFabricantes = await ObterQuantidadeDeFabricantes();
            viewModel.QuantidadeProprietarios = 10;

            return View(viewModel);
        }

        private async Task<int> ObterQuantidadeDeFabricantes()
        {
            var response = await _httpClient.GetAsync("api/fabricantes");

            var contentString = await response.Content.ReadAsStringAsync();

            var fabricantes = JsonConvert.DeserializeObject<List<FabricanteViewModel>>(contentString);

            return fabricantes.Count;
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
