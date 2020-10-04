using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiAmigo.Data;
using WebApiAmigo.Models;

namespace WebApiAmigo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmigoController : ControllerBase
    {
        private readonly IAmigoRepository _repository;

        public AmigoController(IAmigoRepository repository)
        {
            _repository = repository;
        }

        // GET: api/amigo/init
        [HttpGet("init")]
        public string Init() =>  "Iniciou WebApiAmigo";

        [HttpGet]
        public ActionResult<IEnumerable<Amigo>> Get() => Ok(_repository.BuscarAmigos());
        
        // GET api/amigo/5
        [HttpGet("{id}")]
        public ActionResult Get(string id) 
        {
            Amigo amigo = _repository.ObterAmigo(id);

            if(amigo == null)
                return NotFound();

            return Ok(amigo); 
        }

        // POST: api/amigo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Amigo> Post(Amigo amigo)
        {
            amigo.Id = Guid.NewGuid().ToString();
            try
            {
                _repository.AdicionarAmigo(amigo);
                return CreatedAtAction("Get", new { id = amigo.Id });
            } catch(DbUpdateException)
            {
                if(AmigoExists(amigo.Id))
                    return Conflict();
                else
                    throw;
            }
        }

        // PUT api/amigo
        [HttpPut]
        public IActionResult Put(Amigo amigo)
        {
            try
            {
                _repository.EditarAmigo(amigo);
                return CreatedAtAction("Get", new { id = amigo.Id });
            } catch(DbUpdateConcurrencyException)
            {
                if(!AmigoExists(amigo.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // DELETE api/amigo/5
        [HttpDelete("{id}")]
        public ActionResult<Amigo> Delete(string id)
        {
            Amigo amigo = _repository.ObterAmigo(id);

            if (amigo == null)
                return NotFound();

            _repository.ExcluirAmigo(amigo.Id);

            return Ok(amigo);
        }

        [HttpGet("{id}/amigos")]
        public ActionResult GetAmigos(string id)
        {
            var amigosRelacionados = _repository.ObterAmigosRelacionados(id);

            return Ok(amigosRelacionados);
        }

        [HttpPost("amigos")]
        public ActionResult PostAmigos(AmigosRelacionados amigosRelacionados)
        {
            return Ok(_repository.AdicionarAmigosRelacionados(amigosRelacionados));
        }

        // GET: api/amigo/5/exists
        [HttpGet("{id}/exists")]
        public bool AmigoExists(string id) => _repository.ObterAmigo(id) != null;
    }

    
}
