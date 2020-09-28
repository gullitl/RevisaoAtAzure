using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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
            var response = await _httpClient.GetAsync("estado");
            if(response.IsSuccessStatusCode)
                return View(await response.Content.ReadAsAsync<List<EstadoView>>());
            else
                return NotFound();
        }

        // GET: Estado/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
                return NotFound();

            var response = await _httpClient.GetAsync($"estado/{id}");
            if(response.IsSuccessStatusCode) 
            {
                var pais = await response.Content.ReadAsAsync<EstadoView>();
                if(pais == null)
                    return NotFound();

                return View(pais);
            } else
                return NotFound();
        }

        // GET: Estado/Create
        public async Task<IActionResult> Create()
        {
            var response = await _httpClient.GetAsync("pais");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome");
            return View();
        }

        // POST: Estado/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadoId,Nome,PaisId,LogoFile")] EstadoView estado)
        {
            if (ModelState.IsValid)
            {
                var urlLogo = _serviceUpload.Upload(estado.LogoFile);
                estado.FotoBandeira = urlLogo;
                HttpResponseMessage result = await _httpClient.PostAsJsonAsync("estado", estado);

                if(result.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

            }
            var response = await _httpClient.GetAsync("pais");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome", estado.PaisId);
            return View(estado);
        }

        // GET: Estado/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"estado/{id}");
            if(response.IsSuccessStatusCode) {
                var estado = await response.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                var response1 = await _httpClient.GetAsync("pais");
                ViewData["PaisId"] = new SelectList(await response1.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome", estado.PaisId);
                return View(estado);
            } else
                return NotFound();
            
        }

        // POST: Estado/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("EstadoId,Nome,PaisId,LogoFile")] EstadoView estado)
        {
            if (id != estado.EstadoId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var urlLogo = _serviceUpload.Upload(estado.LogoFile);
                    estado.FotoBandeira = urlLogo;
                    await _httpClient.PutAsJsonAsync("estado", estado);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EstadoExists(estado.EstadoId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var response = await _httpClient.GetAsync("pais");
            ViewData["PaisId"] = new SelectList(await response.Content.ReadAsAsync<List<PaisView>>(), "PaisId", "Nome", estado.PaisId);
            return View(estado);
        }

        // GET: Estado/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"estado/{id}");
            if(response.IsSuccessStatusCode) {
                var estado = await response.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                return View(estado);
            } else
                return NotFound();
        }

        // POST: Estado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _httpClient.DeleteAsync($"estado/{id}");
            return RedirectToAction(nameof(Index));
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
