using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Models.Estado;
using WebApp.Models.Pais;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class EstadoController : Controller
    {
        public readonly HttpClient _httpClient;
        public readonly IServiceUpload _serviceUpload;

        public EstadoController(IServiceHttpClientPaisEstado httpClient, IServiceUpload serviceUpload)
        {
            _httpClient = httpClient.GetClient();
            _serviceUpload = serviceUpload;
        }
        
        // GET: Estado
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage responseEstado = await _httpClient.GetAsync("estado");
            if(responseEstado.IsSuccessStatusCode)
            {
                IEnumerable<EstadoView> estados = await responseEstado.Content.ReadAsAsync<IEnumerable<EstadoView>>();

                HttpResponseMessage responsePais = await _httpClient.GetAsync("pais");
                if(responsePais.IsSuccessStatusCode)
                {
                    IEnumerable<PaisView> paises = await responsePais.Content.ReadAsAsync<IEnumerable<PaisView>>();

                    IEnumerable<EstadoView> estadosComPaises = from e in estados
                                           join p in paises on e.PaisId equals p.Id into paisestado
                                           from pe in paisestado.DefaultIfEmpty()
                                           select new EstadoView
                                           {
                                               Id = e.Id,
                                               Nome = e.Nome,
                                               Pais = pe ?? null,
                                               PaisId = pe?.Id ?? string.Empty,
                                               FotoBandeira = e.FotoBandeira
                                           };

                    return View(estadosComPaises);

                } else
                    return NotFound();

                
            } else
                return NotFound();
        }

        // GET: Estado/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
                return NotFound();

            HttpResponseMessage responseEstado = await _httpClient.GetAsync($"estado/{id}");
            if(responseEstado.IsSuccessStatusCode) 
            {
                EstadoView estado = await responseEstado.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                HttpResponseMessage responsePais = await _httpClient.GetAsync($"pais/{estado.PaisId}");
                if(responsePais.IsSuccessStatusCode)
                {
                    estado.Pais = await responsePais.Content.ReadAsAsync<PaisView>();
                    return View(estado);
                } else
                    return NotFound();
                    
            } else
                return NotFound();
        }

        // GET: Estado/Create
        public async Task<IActionResult> Create()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("pais");
            if(response.IsSuccessStatusCode)
            {
                ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome");
                return View();
            }

            return NotFound();
        }

        // POST: Estado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,PaisId,LogoFile")] EstadoView estado)
        {
            if (ModelState.IsValid)
            {
                string urlLogo = _serviceUpload.Upload(estado.LogoFile);
                estado.FotoBandeira = urlLogo;
                HttpResponseMessage result = await _httpClient.PostAsJsonAsync("estado", estado);

                if(result.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

            }
            HttpResponseMessage response = await _httpClient.GetAsync("pais");
            if(response.IsSuccessStatusCode)
                ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome", estado.PaisId);
            
            return View(estado);
        }

        // GET: Estado/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            HttpResponseMessage responseEstado = await _httpClient.GetAsync($"estado/{id}");
            if(responseEstado.IsSuccessStatusCode) {
                EstadoView estado = await responseEstado.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                HttpResponseMessage responsePais = await _httpClient.GetAsync("pais");
                ViewData["PaisId"] = new SelectList(await responsePais.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome", estado.PaisId);
                return View(estado);
            } else
                return NotFound();
            
        }

        // POST: Estado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Nome,PaisId,LogoFile")] EstadoView estado)
        {
            if (id != estado.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    string urlLogo = _serviceUpload.Upload(estado.LogoFile);
                    estado.FotoBandeira = urlLogo;
                    await _httpClient.PutAsJsonAsync("estado", estado);
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EstadoExists(estado.Id))
                        return NotFound();
                    else
                        throw;
                }
            }
            HttpResponseMessage response = await _httpClient.GetAsync("pais");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "Id", "Nome", estado.PaisId);
            return View(estado);
        }

        // GET: Estado/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            HttpResponseMessage responseEstado = await _httpClient.GetAsync($"estado/{id}");
            if(responseEstado.IsSuccessStatusCode) {
                EstadoView estado = await responseEstado.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                HttpResponseMessage responsePais = await _httpClient.GetAsync($"pais/{estado.PaisId}");
                if(responsePais.IsSuccessStatusCode)
                {
                    estado.Pais = await responsePais.Content.ReadAsAsync<PaisView>();
                    return View(estado);
                }
                return NotFound();
                    
            } else
                return NotFound();
        }

        // POST: Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"estado/{id}");
            if(response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));
            return NotFound();
        }

        private async Task<bool> EstadoExists(string id)
        {
            var response = await _httpClient.GetAsync($"estado/{id}/exists");
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<bool>();
            else
                return false;
        }

    }
}
