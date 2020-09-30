using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using WebApiPaisEstado.Models;

namespace WebApiPaisEstado.Data
{
    public class WebPaisEstadoContext : DbContext
    {
        public WebPaisEstadoContext (DbContextOptions<WebPaisEstadoContext> options)
            : base(options)
        {
            Database.EnsureCreatedAsync();
        }
        public IEnumerable<Pais> GetPaisSnapshot()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Pais>>(File.ReadAllText(@"Data/PaisSnapshot.json"));
        }
        public IEnumerable<Estado> GetEstadoSnapshot()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Estado>>(File.ReadAllText(@"Data/EstadoSnapshot.json"));
        }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Estado> Estados { get; set; }
    }
}
