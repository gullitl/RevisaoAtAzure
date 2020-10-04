using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebApiPaisEstado.Data;
using WebApiPaisEstado.Models;

namespace WebApiPaisEstado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly IPaisRepository _repository;

        public PaisController(IPaisRepository repository) 
        {
            _repository = repository;
        }

        // GET: api/pais/init
        [HttpGet("init")]
        public string Init() => "Iniciou WebApiPaisEstado";

        // GET: api/pais
        [HttpGet]
        public ActionResult<IEnumerable<Pais>> Get() => Ok(_repository.BuscarPaises());

        // GET: api/pais/5
        [HttpGet("{id}")]
        public ActionResult<Pais> Get(string id)
        {
            Pais pais = _repository.ObterPais(id);

            if (pais == null)
                return NotFound();

            return Ok(pais);
        }

        // POST: api/pais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Pais> Post(Pais pais)
        {
            pais.Id = Guid.NewGuid().ToString();
            try
            {
                _repository.AdicionarPais(pais);
                return CreatedAtAction("Get", new { id = pais.Id }, pais);
            }
            catch (DbUpdateException)
            {
                if (PaisExists(pais.Id))
                    return Conflict();
                else
                    throw;
            }
        }

        // PUT: api/pais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public IActionResult Put(Pais pais)
        {
            try
            {
                _repository.EditarPais(pais);
                return CreatedAtAction("Get", new { id = pais.Id }, pais);
            } catch(DbUpdateConcurrencyException)
            {
                if(!PaisExists(pais.Id))
                    return NotFound();
                else
                    throw;
            }

        }

        // DELETE: api/pais/5
        [HttpDelete("{id}")]
        public ActionResult<Pais> Delete(string id)
        {
            Pais pais = _repository.ObterPais(id);
            if (pais == null)
                return NotFound();

            _repository.ExcluirPais(pais.Id);

            return Ok(pais);
        }

        // GET: api/pais/5/exists
        [HttpGet("{id}/exists")]
        private bool PaisExists(string id) => _repository.ObterPais(id) != null;
    }
}
