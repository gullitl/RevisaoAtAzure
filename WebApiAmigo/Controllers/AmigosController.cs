using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApiAmigo.Data;
using WebApiAmigo.Models;

namespace WebApiAmigo.Controllers
{
    [Route("api/amigos")]
    [ApiController]
    public class AmigosController : ControllerBase
    {
        public IMapper Mapper { get; }
        public WebApiAmigoContext Context { get; }

        public AmigosController(IMapper mapper, WebApiAmigoContext context)
        {
            Mapper = mapper;
            Context = context;
        }

        [HttpGet]
        public ActionResult<List<AmigoResponse>> Get()
        {
            //if(!Context.Amigo.Any())
            //{
            //    List<Amigo> amigosnapshot = Context.GetAmigoSnapshot();
            //    Context.Amigo.AddRange(amigosnapshot);
            //    Context.SaveChanges();
            //}

            var amigos = Context.Amigo.ToList();

            var response = Mapper.Map<List<AmigoResponse>>(amigos);

            return Ok(response);
        }

        // GET api/<AmigosController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var amigo = Context.Amigo.Find(id);

            var response = Mapper.Map<AmigoResponse>(amigo);

            return Ok(response);
        }

        // POST api/<AmigosController>
        [HttpPost]
        public void Post([FromBody] AmigoRequest request)
        {
            var amigo = Mapper.Map<Amigo>(request);

            Context.Amigo.Add(amigo);
            Context.SaveChanges();
        }

        // PUT api/<AmigosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AmigosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }

        [HttpGet("{id}/amigos")]
        public ActionResult GetAmigos(int id)
        {
            var amigo = Context.Amigo.Where(x => x.Id == id).Include(x => x.Amigos).FirstOrDefault();

            var amigoResponse = Mapper
                .Map<List<AmigoResponse>>(amigo.Amigos.ToList());

            return Ok(amigoResponse);
        }

        [HttpPost("{id}/amigos")]
        public ActionResult PostAmigos([FromRoute]int id, [FromBody] AmigosDoAmigoRequest request)
        {
            var amigo = Context.Amigo.Find(id);
            
            var amigos = Context.Amigo.Where(x => request.AmigosRelacionados.Contains(x.Id)).ToList();

            amigo.Amigos = amigos;

            Context.Update(amigo);
            Context.SaveChanges();

            return Ok();
        }
    }

    public class AmigoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class AmigoRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }

    public class AmigosDoAmigoRequest
    {
        public List<int> AmigosRelacionados { get; set; }
    }
}
