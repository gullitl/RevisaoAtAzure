using Microsoft.EntityFrameworkCore;
using WebApiProprietario.Models;

namespace WebApiProprietario.Data
{
    public class WebApiProprietarioContext : DbContext
    {
        public WebApiProprietarioContext (DbContextOptions<WebApiProprietarioContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Proprietario> Proprietario { get; set; }
    }
}
