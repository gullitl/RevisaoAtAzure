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

        // GET: api/pais
        [HttpGet]
        public string Init() => "Iniciou WebPaisEstado";

        // GET: api/pais
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Pais>>> GetPaises() => await _context.Paises.ToListAsync();

        // GET: api/pais/5
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Pais>> GetPais(string id)
        {
            Pais pais = await _context.Paises.FindAsync(id);

            if (pais == null)
                return NotFound();

            return pais;
        }

        // PUT: api/pais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update")]
        public async Task<IActionResult> PutPais(Pais pais)
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
        [HttpPost("create")]
        public async Task<ActionResult<Pais>> PostPais(Pais pais)
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

            return CreatedAtAction("GetPais", new { id = pais.PaisId }, pais);
        }

        // DELETE: api/pais/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Pais>> DeletePais(string id)
        {
            Pais pais = await _context.Paises.FindAsync(id);
            if (pais == null)
                return NotFound();

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();

            return pais;
        }

        // GET: api/pais/exists/5
        [HttpGet("exists/{id}")]
        private bool PaisExists(string id) => _context.Paises.Any(e => e.PaisId == id);
    }
}
