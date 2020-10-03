using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPaisEstado.Data;
using WebApiPaisEstado.Models;

namespace WebApiPaisEstado.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly WebPaisEstadoContext _context;

        public PaisController(WebPaisEstadoContext context) => _context = context;

        // GET: api/pais/init
        [HttpGet("init")]
        public IEnumerable<Pais> Init()
        {
            return _context.Paises.FromSqlInterpolated($"EXECUTE dbo.BuscarPaises").ToList();
        }

        // GET: api/pais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pais>>> Get() => Ok(await _context.Paises.ToListAsync());

        // GET: api/pais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pais>> Get(string id)
        {
            Pais pais = await _context.Paises.FindAsync(id);

            if (pais == null)
                return NotFound();

            return Ok(pais);
        }

        // POST: api/pais
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Pais>> Post(Pais pais)
        {
            pais.Id = Guid.NewGuid().ToString();
            _context.Paises.Add(pais);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = pais.Id }, pais);
            }
            catch (DbUpdateException)
            {
                if (await PaisExists(pais.Id))
                    return Conflict();
                else
                    throw;
            }
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
                return CreatedAtAction("Get", new { id = pais.Id }, pais);
            } catch(DbUpdateConcurrencyException)
            {
                if(!await PaisExists(pais.Id))
                    return NotFound();
                else
                    throw;
            }

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

            return Ok(pais);
        }

        // GET: api/pais/5/exists
        [HttpGet("{id}/exists")]
        private async Task<bool> PaisExists(string id) => await _context.Paises.AnyAsync(e => e.Id == id);
    }
}
