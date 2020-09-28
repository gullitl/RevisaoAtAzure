using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Amigo;

namespace WebApp.Controllers
{
    public class AmigoController : Controller
    {
        HttpClient httpClient = new HttpClient();

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<AmigoViewModel> amigos = await ObterTodosOsAmigos();

            return View(amigos);
        }

        private async Task<List<AmigoViewModel>> ObterTodosOsAmigos()
        {
            var response = await httpClient.GetAsync($"https://localhost:44395/api/amigos/");

            var content = await response.Content.ReadAsStringAsync();

            var amigos = JsonConvert.DeserializeObject<List<AmigoViewModel>>(content);
            return amigos;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(object form)
        {
            return View();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> RelacionarAmigos([FromRoute] int id)
        {
            var viewModel = new RelacionarAmigosViewModel();

            var response = await httpClient.GetAsync($"https://localhost:44395/api/amigos/{id}/amigos");

            var content = await response.Content.ReadAsStringAsync();

            viewModel.TodosAmigos = await ObterTodosOsAmigos();

            viewModel.Amigo = viewModel.TodosAmigos.First(x => x.Id == id);

            viewModel.TodosAmigos.Remove(viewModel.Amigo);

            var amigosRelacionados = JsonConvert.DeserializeObject<List<AmigoViewModel>>(content).Select(x => x.Id);

            viewModel.AmigosRelacionados = amigosRelacionados.ToList();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> RelacionarAmigos([FromForm]RelacionarAmigosViewModel form)
        {
            var amigosJson = JsonConvert.SerializeObject(form);

            var stringContent = new StringContent(amigosJson, Encoding.UTF8, "application/json");

            await httpClient.PostAsync($"https://localhost:44395/api/amigos/{form.Amigo.Id}/amigos", stringContent);

            return RedirectToAction(nameof(RelacionarAmigos), new { form.Amigo.Id });
        }

    }
}
