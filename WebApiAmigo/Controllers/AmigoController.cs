using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiAmigo.Data;
using WebApiAmigo.Models;

namespace WebApiAmigo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigoController : ControllerBase
    {
        public IMapper Mapper { get; }
        public readonly WebApiAmigoContext _context;

        public AmigoController(IMapper mapper, WebApiAmigoContext context)
        {
            Mapper = mapper;
            _context = context;
        }

        // GET: api/amigo/init
        [HttpGet("init")]
        public async Task<string> Init()
        {
            if(!await _context.Amigos.AnyAsync())
            {
                IEnumerable<Amigo> amigosnapshot = _context.GetAmigoSnapshot();
                _context.Amigos.AddRange(amigosnapshot);
                _context.SaveChanges();
            }

            return "Iniciou WebApiAmigo";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Amigo>>> Get() => Ok(await _context.Amigos.ToListAsync());

        // GET api/amigo/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id) 
        {
            Amigo amigo = await _context.Amigos.FindAsync(id);

            if(amigo == null)
                return NotFound();

            return Ok(amigo); 
        }

        // POST: api/amigo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Amigo>> Post(Amigo amigo)
        {
            amigo.Id = Guid.NewGuid().ToString();
            _context.Amigos.Add(amigo);
            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = amigo.Id });
            } catch(DbUpdateException)
            {
                if(await AmigoExists(amigo.Id))
                    return Conflict();
                else
                    throw;
            }
        }

        // PUT api/amigo
        [HttpPut]
        public async Task<IActionResult> Put(Amigo amigo)
        {
            _context.Entry(amigo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("Get", new { id = amigo.Id });
            } catch(DbUpdateConcurrencyException)
            {
                if(!await AmigoExists(amigo.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // DELETE api/amigo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Amigo>> Delete(string id)
        {
            Amigo amigo = await _context.Amigos.FindAsync(id);

            if(amigo == null)
                return NotFound();

            _context.Amigos.Remove(amigo);
            await _context.SaveChangesAsync();

            return amigo;
        }

        [HttpGet("{id}/amigos")]
        public ActionResult GetAmigos(string id)
        {
            Amigo amigo = _context.Amigos.Where(x => x.Id == id).Include(x => x.Amigos).FirstOrDefaultAsync().Result;
            return Ok(amigo);
        }

        [HttpPost("{id}/amigos")]
        public async Task<ActionResult> PostAmigos(string id, AmigosRelacionados amigosRelacionados)
        {
            List<Amigo> amigos = await _context.Amigos.Where(x => amigosRelacionados.AmigosId.Contains(x.Id)).ToListAsync();
            
            Amigo amigo = await _context.Amigos.FindAsync(id);
            amigo.Amigos = amigos;

            _context.Update(amigo);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // GET: api/amigo/5/exists
        [HttpGet("{id}/exists")]
        public async Task<bool> AmigoExists(string id) => await _context.Amigos.AnyAsync(e => e.Id == id);
    }

    
}
