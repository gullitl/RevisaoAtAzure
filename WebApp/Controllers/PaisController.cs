﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net.Http;
using System.Threading.Tasks;
using WebApp.Models.Pais;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class PaisController : Controller
    {
        public readonly HttpClient _httpClient;
        public readonly IServiceUpload _serviceUpload;

        public PaisController(IServiceHttpClientPaisEstado httpClient, IServiceUpload serviceUpload)
        {
            _httpClient = httpClient.GetClient();
            _serviceUpload = serviceUpload;
        }
        // GET: Pais
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("pais");
            if(response.IsSuccessStatusCode) 
                return View(await response.Content.ReadAsAsync<List<PaisView>>());
            else
                return NotFound();
        }

        // GET: Pais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var response = await _httpClient.GetAsync($"pais/{id}");
            if(response.IsSuccessStatusCode) 
            {
                var pais = await response.Content.ReadAsAsync<PaisView>();
                if(pais == null)
                    return NotFound();
                
                return View(pais);
            } 
            else 
                return NotFound();
            
        }

        // GET: Pais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaisId,Nome,LogoFile")] PaisView pais)
        {
            if (ModelState.IsValid)
            {
                var urlLogo = _serviceUpload.Upload(pais.LogoFile);
                pais.FotoBandeira = urlLogo;
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"pais", pais);

                if(response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }

        // GET: Pais/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();

            var response = await _httpClient.GetAsync($"pais/{id}");
            if(response.IsSuccessStatusCode) 
            {
                var pais = await response.Content.ReadAsAsync<PaisView>();
                if(pais == null)
                    return NotFound();

                return View(pais);
            } else
                return NotFound();
        }

        // POST: Pais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("PaisId,Nome,LogoFile")] PaisView pais)
        {
            if (id != pais.PaisId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var urlLogo = _serviceUpload.Upload(pais.LogoFile);
                    pais.FotoBandeira = urlLogo;
                    await _httpClient.PutAsJsonAsync($"pais", pais);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PaisExists(pais.PaisId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }

        // GET: Pais/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var response = await _httpClient.GetAsync($"pais/{id}");
            if(response.IsSuccessStatusCode) 
            {
                var pais = await response.Content.ReadAsAsync<PaisView>();
                if(pais == null)
                    return NotFound();

                return View(pais);
            } else
                return NotFound();
        }

        // POST: Pais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _httpClient.DeleteAsync($"pais{id}");
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PaisExists(string id)
        {
            var response = await _httpClient.GetAsync($"pais/{id}/exists");
            if(response.IsSuccessStatusCode) 
            {
                return await response.Content.ReadAsAsync<bool>();
            } else
                return false;
        }

    }
}
