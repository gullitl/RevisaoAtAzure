using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Estado>>> Get() => Ok(await _context.Estados.ToListAsync());

        // GET: api/estado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> Get(string id)
        {
            Estado estado = await _context.Estados.FindAsync(id);

            if (estado == null)
                return NotFound();

            return estado;
        }

        // POST: api/estado
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Estado>> Post(Estado estado)
        {
            estado.Id = Guid.NewGuid().ToString();
            _context.Estados.Add(estado);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = estado.Id });
            } catch(DbUpdateException)
            {
                if(await EstadoExists(estado.Id))
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
            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = estado.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EstadoExists(estado.Id))
                    return NotFound();
                else
                    throw;
            }

        }

        // DELETE: api/estado/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Estado>> Delete(string id)
        {
            Estado estado = await _context.Estados.FindAsync(id);

            if (estado == null)
                return NotFound();

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return estado;
        }

        // GET: api/estado/5/exists
        [HttpGet("{id}/exists")]
        public async Task<bool> EstadoExists(string id) => await _context.Estados.AnyAsync(e => e.Id == id);
    }
}
