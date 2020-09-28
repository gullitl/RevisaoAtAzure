using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Amigo;
using WebApp.Models.Home;
using WebApp.Models.Pais;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public readonly HttpClient _httpClientAmigo;
        public readonly HttpClient _httpClientPaisEstado;

        public HomeController(IServiceHttpClientAmigo httpClientAmigo, 
            IServiceHttpClientPaisEstado httpClientPaisEstado)
        {
            _httpClientAmigo = httpClientAmigo.GetClient();
            _httpClientPaisEstado = httpClientPaisEstado.GetClient();
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                QtdAmigos = await GetQtdAmigos(),
                QtdPaises = await GetQtdPaises(),
                QtdEstados = await GetQtdEstados()
            };

            if(viewModel.IsValid)
                return View(viewModel);
            else
                return NotFound();
        }

        private async Task<int> GetQtdAmigos()
        {
            var response = await _httpClientAmigo.GetAsync("");

            if(response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<AmigoView>>().Result.Count();
            return -1;
        }

        private async Task<int> GetQtdPaises()
        {
            var response = await _httpClientPaisEstado.GetAsync("pais");

            if(response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<PaisView>>().Result.Count();
            return -1;
        }

        private async Task<int> GetQtdEstados()
        {
            var response = await _httpClientPaisEstado.GetAsync("estado");

            if(response.IsSuccessStatusCode)
                return response.Content.ReadAsAsync<IEnumerable<PaisView>>().Result.Count();
            return -1;
        }

        public IActionResult Sobre() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
