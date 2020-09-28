using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Amigo;
using WebApp.Models.Estado;
using WebApp.Models.Pais;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class AmigoController : Controller
    {
        public readonly HttpClient _httpClient;
        public readonly IServiceUpload _serviceUpload;

        public AmigoController(IServiceHttpClientAmigo httpClient, IServiceUpload serviceUpload)
        {
            _httpClient = httpClient.GetClient();
            _serviceUpload = serviceUpload;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<AmigoViewModel> amigos = await ObterTodosOsAmigos();

            return View(amigos);
        }

        private async Task<List<AmigoViewModel>> ObterTodosOsAmigos()
        {
            var response = await _httpClient.GetAsync("");

            var content = await response.Content.ReadAsStringAsync();

            var amigos = JsonConvert.DeserializeObject<List<AmigoViewModel>>(content);
            return amigos;
        }

        // GET: Amigo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
                return NotFound();

            var response = await _httpClient.GetAsync($"{id}");
            if(response.IsSuccessStatusCode)
            {
                var amigo = await response.Content.ReadAsAsync<AmigoView>();
                if(amigo == null)
                    return NotFound();

                return View(amigo);
            } else
                return NotFound();
        }

        // GET: Amigo/Create
        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome");
            ViewData["EstadoId"] = new SelectList(await response.Content.ReadAsAsync<List<EstadoView>>(), "EstadoId", "Nome");
            return View();
        }

        // POST: Amigo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmigoId,Nome,Sobrenome,Email,Telefone,PaisId,EstadoId,FotoFile")] AmigoView amigo)
        {
            if(ModelState.IsValid)
            {
                var urlLogo = _serviceUpload.Upload(amigo.FotoFile);
                amigo.Foto = urlLogo;
                HttpResponseMessage result = await _httpClient.PostAsJsonAsync("", amigo);

                if(result.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

            }
            var response = await _httpClient.GetAsync("");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome", amigo.PaisId);
            ViewData["EstadoId"] = new SelectList(await response.Content.ReadAsAsync<List<EstadoView>>(), "EstadoId", "Nome");
            return View(amigo);
        }


        // GET: Amigo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{id}");
            if(response.IsSuccessStatusCode)
            {
                var amigo = await response.Content.ReadAsAsync<AmigoView>();
                if(amigo == null)
                    return NotFound();

                var response1 = await _httpClient.GetAsync("");
                ViewData["PaisId"] = new SelectList(await response1.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome", amigo.PaisId);
                ViewData["EstadoId"] = new SelectList(await response1.Content.ReadAsAsync<List<EstadoView>>(), "EstadoId", "Nome", amigo.EstadoId);
                return View(amigo);
            } else
                return NotFound();

        }

        // POST: Amigo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AmigoId,Nome,Sobrenome,Email,Telefone,PaisId,EstadoId,FotoFile")] AmigoView amigo)
        {
            if(id != amigo.AmigoId)
                return NotFound();

            if(ModelState.IsValid)
            {
                try
                {
                    var urlLogo = _serviceUpload.Upload(amigo.FotoFile);
                    amigo.Foto = urlLogo;
                    await _httpClient.PutAsJsonAsync("", amigo);
                } catch(DbUpdateConcurrencyException)
                {
                    if(!await AmigoExists(amigo.EstadoId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var response = await _httpClient.GetAsync("");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome", amigo.PaisId);
            ViewData["EstadoId"] = new SelectList(await response.Content.ReadAsAsync<List<EstadoView>>(), "EstadoId", "Nome", amigo.EstadoId);
            return View(amigo);
        }

        // GET: Amigo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"{id}");
            if(response.IsSuccessStatusCode)
            {
                var estado = await response.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                return View(estado);
            } else
                return NotFound();
        }

        // POST: Amigo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _httpClient.DeleteAsync($"{id}");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> RelacionarAmigos([FromRoute] int id)
        {
            var viewModel = new RelacionarAmigosViewModel();

            var response = await _httpClient.GetAsync($"{id}/amigos");

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

            await _httpClient.PostAsync($"{form.Amigo.Id}/amigos", stringContent);

            return RedirectToAction(nameof(RelacionarAmigos), new { form.Amigo.Id });
        }

        private async Task<bool> AmigoExists(string id)
        {
            var response = await _httpClient.GetAsync($"{id}/exists");
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<bool>();
            else
                return false;
        }

    }
}
