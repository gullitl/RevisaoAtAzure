using Microsoft.EntityFrameworkCore;
using WebPaisEstado.Models;

namespace WebPaisEstado.Data
{
    public class WebPaisEstadoContext : DbContext
    {
        public WebPaisEstadoContext (DbContextOptions<WebPaisEstadoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Pais> Paises { get; set; }

        public DbSet<Estado> Estados { get; set; }
    }
}
