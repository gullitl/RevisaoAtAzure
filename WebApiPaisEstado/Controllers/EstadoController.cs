using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebPaisEstado.Data;
using WebPaisEstado.Models;

namespace WebPaisEstado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly WebPaisEstadoContext _context;

        public EstadoController(WebPaisEstadoContext context) => _context = context;

        // GET: api/estado
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstados() => await _context.Estados.ToListAsync();

        // GET: api/estado/getbyid/5
        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<Estado>> GetEstado(string id)
        {
            Estado estado = await _context.Estados.FindAsync(id);

            if (estado == null)
                return NotFound();

            return estado;
        }

        // PUT: api/estado
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update")]
        public async Task<IActionResult> PutEstado(Estado estado)
        {
            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(estado.EstadoId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/estado
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create")]
        public async Task<ActionResult<Estado>> PostEstado(Estado estado)
        {
            estado.EstadoId = Guid.NewGuid().ToString();
            _context.Estados.Add(estado);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EstadoExists(estado.EstadoId))
                    return Conflict();
                else
                    throw;
            }

            return CreatedAtAction("GetEstado", new { id = estado.EstadoId }, estado);
        }

        // DELETE: api/estado/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Estado>> DeleteEstado(string id)
        {
            Estado estado = await _context.Estados.FindAsync(id);

            if (estado == null)
                return NotFound();

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return estado;
        }

        // GET: api/estado/exists/5
        [HttpGet("exists/{id}")]
        private bool EstadoExists(string id) => _context.Estados.Any(e => e.EstadoId == id);
    }
}
