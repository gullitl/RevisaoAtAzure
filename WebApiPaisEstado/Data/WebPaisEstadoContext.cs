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
        }
       
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Estado> Estados { get; set; }
    }
}
