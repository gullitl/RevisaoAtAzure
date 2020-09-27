using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models.Fabricante;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace WebApp.Controllers
{
    public class FabricanteController : Controller
    {
        private readonly HttpClient _httpClient;

        public FabricanteController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:61347");
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new FabricanteIndexViewModel();

            var response = await _httpClient.GetAsync("api/fabricantes");

            var contentString = await response.Content.ReadAsStringAsync();

            viewModel.Fabricantes = JsonConvert.DeserializeObject<List<FabricanteViewModel>>(contentString);

            return View(viewModel);
        }

        // GET: FabricanteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FabricanteController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CriarFabricanteViewModel viewModel)
        {
            try
            {
                var urlLogo = Upload(viewModel.LogoFile);
                viewModel.Logo = urlLogo;

                var viewModelJson = JsonConvert.SerializeObject(viewModel);

                var dados = new StringContent(viewModelJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/fabricantes", dados);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(viewModel);
            }
            catch
            {
                return View(viewModel);
            }
        }

        private string Upload(IFormFile logoFile)
        {
            var reader = logoFile.OpenReadStream();
            var cloudStorageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=revisaoazure;AccountKey=pIRWbBZIntC3YZ+0PXpNNb9DlHxyYUGA039hFx1TePl0PmoK+OKwaRs20fXdSrKUClsfDRg7onNizE/nTqqnzQ==;EndpointSuffix=core.windows.net");
            var blobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("post-images");
            container.CreateIfNotExists();
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.UploadFromStream(reader);
            var destinoDaImagemNaNuvem = blob.Uri.ToString();
            return destinoDaImagemNaNuvem;
        }

        // GET: FabricanteController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var response = await _httpClient.GetAsync("api/fabricantes/" + id);

            var contentString = await response.Content.ReadAsStringAsync();

            var viewModel = JsonConvert.DeserializeObject<AlterarFabricanteViewModel>(contentString);

            return View(viewModel);
        }

        // POST: FabricanteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AlterarFabricanteViewModel viewModel)
        {
            try
            {
                var urlLogo = Upload(viewModel.LogoFile);
                viewModel.Logo = urlLogo;

                var viewModelJson = JsonConvert.SerializeObject(viewModel);

                var dados = new StringContent(viewModelJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync("api/fabricantes/" + viewModel.Id, dados);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(string id)
        {
            var response = await _httpClient.GetAsync("api/fabricantes/" + id);

            var contentString = await response.Content.ReadAsStringAsync();

            var viewModel = JsonConvert.DeserializeObject<FabricanteDeleteViewModel>(contentString);

            return View(viewModel);
        }

        // POST: FabricanteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, FabricanteDeleteViewModel viewModel)
        {
            try
            {
                var response = await _httpClient.DeleteAsync("api/fabricantes/" + id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
