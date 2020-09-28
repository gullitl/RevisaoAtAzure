using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
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
        private readonly string estadoRoute = "api/estado";
        private readonly string paisRoute = "api/pais";

        public EstadoController(IServiceHttpClientPaisEstado httpClient) => _httpClient = httpClient.GetClient();

        // GET: Estado
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync($"{estadoRoute}/getall");
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

            var response = await _httpClient.GetAsync($"{estadoRoute}/getbyid/{id}");
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
            var response = await _httpClient.GetAsync($"{paisRoute}/getall");
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
                var urlLogo = Upload(estado.LogoFile);
                estado.FotoBandeira = urlLogo;
                HttpResponseMessage result = await _httpClient.PostAsJsonAsync($"{estadoRoute}/create", estado);

                if(result.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

            }
            var response = await _httpClient.GetAsync($"{paisRoute}/getall");
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

            var response = await _httpClient.GetAsync($"{estadoRoute}/getbyid/{id}");
            if(response.IsSuccessStatusCode) {
                var estado = await response.Content.ReadAsAsync<EstadoView>();
                if(estado == null)
                    return NotFound();

                var response1 = await _httpClient.GetAsync($"{paisRoute}/getall");
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
                    var urlLogo = Upload(estado.LogoFile);
                    estado.FotoBandeira = urlLogo;
                    await _httpClient.PutAsJsonAsync($"{estadoRoute}/update", estado);
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
            var response = await _httpClient.GetAsync($"{paisRoute}/getall");
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

            var response = await _httpClient.GetAsync($"{estadoRoute}/getbyid/{id}");
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
            await _httpClient.DeleteAsync($"{estadoRoute}/delete/{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EstadoExists(string id)
        {
            var response = await _httpClient.GetAsync($"{estadoRoute}/exists/{id}");
            if(response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<bool>();
            } else
                return false;
        }

        private string Upload(IFormFile logoFile)
        {
            var reader = logoFile.OpenReadStream();
            var cloudStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=hortananuvem;AccountKey=dJ5gWC6luF4EmaTy1F1HUPUIr0lK3faspJB6rajYfym8fgBZdDxi3x5aeRy0apnIrogIRWahJfghyRoERV26Hw==;EndpointSuffix=core.windows.net");
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("post-images");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();
            return destinoDaImagemNaNuvem;
        }
    }
}
