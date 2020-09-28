using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public string Init() => "Iniciou WebApiAmigo";

        [HttpGet]
        public ActionResult<List<AmigoResponse>> Get()
        {
            //if(!Context.Amigo.Any())
            //{
            //    List<Amigo> amigosnapshot = Context.GetAmigoSnapshot();
            //    Context.Amigo.AddRange(amigosnapshot);
            //    Context.SaveChanges();
            //}

            var amigos = _context.Amigos.ToList();

            var response = Mapper.Map<List<AmigoResponse>>(amigos);

            return Ok(response);
        }

        // GET api/amigo/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var amigo = _context.Amigos.Find(id);

            var response = Mapper.Map<AmigoResponse>(amigo);

            return Ok(response);
        }

        // POST api/amigo
        [HttpPost]
        public void Post([FromBody] AmigoRequest request)
        {
            var amigo = Mapper.Map<Amigo>(request);

            _context.Amigos.Add(amigo);
            _context.SaveChanges();
        }

        // PUT api/amigo
        [HttpPut]
        public async Task<IActionResult> Put(Amigo amigo)
        {
            _context.Entry(amigo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException)
            {
                if(!AmigoExists(amigo.EstadoId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
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
            var amigo = _context.Amigos.Where(x => x.AmigoId == id).Include(x => x.Amigos).FirstOrDefault();

            var amigoResponse = Mapper
                .Map<List<AmigoResponse>>(amigo.Amigos.ToList());

            return Ok(amigoResponse);
        }

        [HttpPost("{id}/amigos")]
        public ActionResult PostAmigos([FromRoute]int id, [FromBody] AmigosDoAmigoRequest request)
        {
            var amigo = _context.Amigos.Find(id);
            
            var amigos = _context.Amigos.Where(x => request.AmigosRelacionados.Contains(x.AmigoId)).ToList();

            amigo.Amigos = amigos;

            _context.Update(amigo);
            _context.SaveChanges();

            return Ok();
        }

        // GET: api/amigo/5/exists
        [HttpGet("{id}/exists")]
        private bool AmigoExists(string id) => _context.Amigos.Any(e => e.AmigoId == id);
    }

    
}
