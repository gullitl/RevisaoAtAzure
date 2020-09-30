using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WebApiAmigo.Models;

namespace WebApiAmigo.Data
{
    public class WebApiAmigoContext : DbContext
    {
        public WebApiAmigoContext (DbContextOptions<WebApiAmigoContext> options)
            : base(options) => Database.EnsureCreatedAsync();

        public IEnumerable<Amigo> GetAmigoSnapshot()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Amigo>>(File.ReadAllText(@"Data/AmigoSnapshot.json"));
        }
        public IEnumerable<AmigosRelacionados> GetAmigosRelacionadosSnapshot()
        {
            return JsonConvert.DeserializeObject<IEnumerable<AmigosRelacionados>>(File.ReadAllText(@"Data/AmigosRelacionadosSnapshot.json"));
        }
        public DbSet<Amigo> Amigos { get; set; }


    }
}
