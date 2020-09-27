using Microsoft.EntityFrameworkCore;

namespace WebFabricante.Data
{
    public class WebFabricanteContext : DbContext
    {
        public WebFabricanteContext (DbContextOptions<WebFabricanteContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<WebFabricante.Models.Fabricante> Fabricante { get; set; }

        public DbSet<WebFabricante.Models.Carro> Carro { get; set; }
    }
}
