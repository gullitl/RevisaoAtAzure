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
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Amigo> Amigo { get; set; }
        //public DbSet<AmigosRelacionado> AmigosRelacionados { get; set; }


        public List<Amigo> GetAmigoSnapshot() => JsonConvert.DeserializeObject<List<Amigo>>(File.ReadAllText(@"Data/AmigoSnapshot.json"));

    }
}
