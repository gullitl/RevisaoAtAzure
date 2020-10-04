using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiPaisEstado.Data;
using WebApiPaisEstado.Models;

namespace WebApiPaisEstado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoRepository _repository;

        public EstadoController(IEstadoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/estado
        [HttpGet()]
        public ActionResult<IEnumerable<Estado>> Get() => Ok(_repository.BuscarEstados());

        // GET: api/estado/5
        [HttpGet("{id}")]
        public ActionResult<Estado> Get(string id)
        {
            Estado estado = _repository.ObterEstado(id);

            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        // POST: api/estado
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Estado> Post(Estado estado)
        {
            estado.Id = Guid.NewGuid().ToString();
            try
            {
                _repository.AdicionarEstado(estado);
                return CreatedAtAction("Get", new { id = estado.Id }, estado);
            } catch(DbUpdateException)
            {
                if(EstadoExists(estado.Id))
                    return Conflict();
                else
                    throw;
            }

        }

        // PUT: api/estado
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> Put(Estado estado)
        {
            try
            {
                _repository.EditarEstado(estado);
                return CreatedAtAction("Get", new { id = estado.Id }, estado);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(estado.Id))
                    return NotFound();
                else
                    throw;
            }

        }

        // DELETE: api/estado/5
        [HttpDelete("{id}")]
        public ActionResult<Estado> Delete(string id)
        {
            Estado estado = _repository.ObterEstado(id);

            if (estado == null)
                return NotFound();

            _repository.ExcluirEstado(estado.Id);

            return Ok(estado);
        }

        // GET: api/estado/5/exists
        [HttpGet("{id}/exists")]
        public bool EstadoExists(string id) => _repository.ObterEstado(id) != null;
    }
}
