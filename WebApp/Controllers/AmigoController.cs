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
        public readonly HttpClient _httpClientAmigo;
        public readonly HttpClient _httpClientPaisEstado;
        public readonly IServiceUpload _serviceUpload;

        public AmigoController(IServiceHttpClientAmigo httpClientAmigo,
            IServiceHttpClientPaisEstado httpClientPaisEstado,
            IServiceUpload serviceUpload)
        {
            _httpClientAmigo = httpClientAmigo.GetClient();
            _httpClientPaisEstado = httpClientPaisEstado.GetClient();
            _serviceUpload = serviceUpload;
        }

        // GET: Amigo
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage responseAmigo = await _httpClientAmigo.GetAsync("");
            if(responseAmigo.IsSuccessStatusCode)
            {
                IEnumerable<AmigoView> amigos = await responseAmigo.Content.ReadAsAsync<IEnumerable<AmigoView>>();

                HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync("pais");
                if(responsePais.IsSuccessStatusCode)
                {
                    IEnumerable<PaisView> paises = await responsePais.Content.ReadAsAsync<IEnumerable<PaisView>>();

                    HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync("estado");
                    if(responseEstado.IsSuccessStatusCode)
                    {
                        IEnumerable<EstadoView> estados = await responseEstado.Content.ReadAsAsync<IEnumerable<EstadoView>>();

                        IEnumerable<AmigoView> amigosComPaisesEEstados = from a in amigos
                                                                        join p in paises on a.PaisId equals p.Id into paisamigo
                                                                        join e in estados on a.EstadoId equals e.Id into estadoamigo
                                                                        from pa in paisamigo.DefaultIfEmpty()
                                                                        from ea in estadoamigo.DefaultIfEmpty()
                                                                        select new AmigoView
                                                                        {
                                                                            Id = a.Id,
                                                                            Nome = a.Nome,
                                                                            Sobrenome = a.Sobrenome,
                                                                            Email = a.Email,
                                                                            Telefone = a.Telefone,
                                                                            DataNascimento = a.DataNascimento,
                                                                            Pais = pa ?? null,
                                                                            PaisId = pa?.Id ?? string.Empty,
                                                                            Estado = ea ?? null,
                                                                            EstadoId = ea?.Id ?? string.Empty,
                                                                            Foto = a.Foto
                                                                        };

                        return View(amigosComPaisesEEstados);

                    }
                    return NotFound();

                } else
                    return NotFound();

            } else
                return NotFound();
        }

        private async Task<IEnumerable<AmigoView>> ObterTodosOsAmigos()
        {
            HttpResponseMessage responseAmigo = await _httpClientAmigo.GetAsync("");
            if(responseAmigo.IsSuccessStatusCode)
                return await responseAmigo.Content.ReadAsAsync<IEnumerable<AmigoView>>();
            
            return null;
        }

        // GET: Amigo/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
                return NotFound();

            HttpResponseMessage responseAmigo = await _httpClientAmigo.GetAsync($"{id}");
            if(responseAmigo.IsSuccessStatusCode)
            {
                AmigoView amigo = await responseAmigo.Content.ReadAsAsync<AmigoView>();
                if(amigo == null)
                    return NotFound();
              
                HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync($"pais/{amigo.PaisId}");
                if(responsePais.IsSuccessStatusCode)
                    amigo.Pais = await responsePais.Content.ReadAsAsync<PaisView>();
                
                HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync($"estado/{amigo.EstadoId}");
                if(responseEstado.IsSuccessStatusCode)
                    amigo.Estado = await responseEstado.Content.ReadAsAsync<EstadoView>();

                return View(amigo);

            } else
                return NotFound();
        }

        // GET: Amigo/Create
        public async Task<IActionResult> Create()
        {
            HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync("pais");
            if(responsePais.IsSuccessStatusCode)
                ViewData["PaisId"] = new SelectList(await responsePais.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome");
            else
                return NotFound();

            HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync("estado");
            if(responseEstado.IsSuccessStatusCode)
                ViewData["EstadoId"] = new SelectList(await responseEstado.Content.ReadAsAsync<List<EstadoView>>(), "Id", "Nome");
            else
                return NotFound();

            return View();
        }

        // POST: Amigo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Sobrenome,Email,Telefone,PaisId,EstadoId,FotoFile")] AmigoView amigo)
        {
            if(ModelState.IsValid)
            {
                string urlLogo = _serviceUpload.Upload(amigo.FotoFile);
                amigo.Foto = urlLogo;
                HttpResponseMessage result = await _httpClientAmigo.PostAsJsonAsync("", amigo);

                if(result.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

            }
            HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync("pais");
            if(responsePais.IsSuccessStatusCode)
                ViewData["PaisId"] = new SelectList(await responsePais.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome", amigo.PaisId);

            HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync("estado");
            if(responseEstado.IsSuccessStatusCode)
                ViewData["EstadoId"] = new SelectList(await responseEstado.Content.ReadAsAsync<List<EstadoView>>(), "Id", "Nome");
            
            return View(amigo);
        }

        // GET: Amigo/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
                return NotFound();

            HttpResponseMessage response = await _httpClientAmigo.GetAsync($"{id}");
            if(response.IsSuccessStatusCode)
            {
                AmigoView amigo = await response.Content.ReadAsAsync<AmigoView>();
                if(amigo == null)
                    return NotFound();

                HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync("pais");
                if(responsePais.IsSuccessStatusCode)
                    ViewData["PaisId"] = new SelectList(await responsePais.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome", amigo.PaisId);

                HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync("estado");
                if(responseEstado.IsSuccessStatusCode)
                    ViewData["EstadoId"] = new SelectList(await responseEstado.Content.ReadAsAsync<List<EstadoView>>(), "Id", "Nome");

                return View(amigo);
            } else
                return NotFound();

        }

        // POST: Amigo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,Sobrenome,Email,Telefone,PaisId,EstadoId,FotoFile")] AmigoView amigo)
        {
            if(id != amigo.Id)
                return NotFound();

            if(ModelState.IsValid)
            {
                try
                {
                    string urlLogo = _serviceUpload.Upload(amigo.FotoFile);
                    amigo.Foto = urlLogo;
                    await _httpClientAmigo.PutAsJsonAsync("", amigo);
                    return RedirectToAction(nameof(Index));
                } catch(DbUpdateConcurrencyException)
                {
                    if(!await AmigoExists(amigo.EstadoId))
                        return NotFound();
                    else
                        throw;
                }
            }
            HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync("pais");
            if(responsePais.IsSuccessStatusCode)
                ViewData["PaisId"] = new SelectList(await responsePais.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome", amigo.PaisId);

            HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync("estado");
            if(responseEstado.IsSuccessStatusCode)
                ViewData["EstadoId"] = new SelectList(await responseEstado.Content.ReadAsAsync<List<EstadoView>>(), "Id", "Nome");

            return View(amigo);
        }

        // GET: Amigo/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if(id == null)
                return NotFound();

            HttpResponseMessage responseAmigo = await _httpClientAmigo.GetAsync($"{id}");
            if(responseAmigo.IsSuccessStatusCode)
            {
                AmigoView amigo = await responseAmigo.Content.ReadAsAsync<AmigoView>();
                if(amigo == null)
                    return NotFound();

                HttpResponseMessage responsePais = await _httpClientPaisEstado.GetAsync($"pais/{amigo.PaisId}");
                if(responsePais.IsSuccessStatusCode)
                    amigo.Pais = await responsePais.Content.ReadAsAsync<PaisView>();

                HttpResponseMessage responseEstado = await _httpClientPaisEstado.GetAsync($"estado/{amigo.EstadoId}");
                if(responseEstado.IsSuccessStatusCode)
                    amigo.Estado = await responseEstado.Content.ReadAsAsync<EstadoView>();

                return View(amigo);
            } else
                return NotFound();
        }

        // POST: Amigo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpResponseMessage response = await _httpClientAmigo.DeleteAsync($"{id}");
            if(response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> RelacionarAmigos([FromRoute] string id)
        {
            HttpResponseMessage responseAmigo = await _httpClientAmigo.GetAsync($"{id}/amigos");
            if(responseAmigo.IsSuccessStatusCode)
            {
                AmigosRelacionadosView viewModel = await responseAmigo.Content.ReadAsAsync<AmigosRelacionadosView>();
                return View(viewModel);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> RelacionarAmigos([FromForm]AmigosRelacionadosView amigosRelacionados)
        {
            HttpResponseMessage result = await _httpClientAmigo.PostAsJsonAsync("amigos", amigosRelacionados);

            if(result.IsSuccessStatusCode)
                return RedirectToAction(nameof(RelacionarAmigos), new { amigosRelacionados.Amigo.Id });

            return NotFound();
        }

        private async Task<bool> AmigoExists(string id)
        {
            var response = await _httpClientAmigo.GetAsync($"{id}/exists");
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<bool>();
            else
                return false;
        }

    }
}
