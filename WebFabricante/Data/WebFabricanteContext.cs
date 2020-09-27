using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebFabricante.Models;

namespace WebFabricante.Data
{
    public class WebFabricanteContext : DbContext
    {
        public WebFabricanteContext (DbContextOptions<WebFabricanteContext> options)
            : base(options)
        {
        }

        public DbSet<WebFabricante.Models.Fabricante> Fabricante { get; set; }

        public DbSet<WebFabricante.Models.Carro> Carro { get; set; }
    }
}
