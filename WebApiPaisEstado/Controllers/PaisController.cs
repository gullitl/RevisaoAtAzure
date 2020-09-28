using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPaisEstado.Data;
using WebPaisEstado.Models;

namespace WebPaisEstado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly WebPaisEstadoContext _context;

        public PaisController(WebPaisEstadoContext context) => _context = context;

        // GET: api/pais/init
        [HttpGet]
        public string Init() => "Iniciou WebApiPaisEstado";

        // GET: api/pais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pais>>> Get() => await _context.Paises.ToListAsync();

        // GET: api/pais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pais>> Get(string id)
        {
            Pais pais = await _context.Paises.FindAsync(id);

            if (pais == null)
                return NotFound();

            return pais;
        }

        // PUT: api/pais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> Put(Pais pais)
        {
            _context.Entry(pais).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(pais.PaisId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/pais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pais>> Post(Pais pais)
        {
            pais.PaisId = Guid.NewGuid().ToString();
            _context.Paises.Add(pais);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaisExists(pais.PaisId))
                    return Conflict();
                else
                    throw;
            }

            return CreatedAtAction("Get", new { id = pais.PaisId }, pais);
        }

        // DELETE: api/pais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pais>> Delete(string id)
        {
            Pais pais = await _context.Paises.FindAsync(id);
            if (pais == null)
                return NotFound();

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return pais;
        }

        // GET: api/pais/5/exists
        [HttpGet("{id}/exists")]
        private bool PaisExists(string id) => _context.Paises.Any(e => e.PaisId == id);
    }
}
