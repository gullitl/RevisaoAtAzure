using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiProprietario.Models;

namespace WebApiProprietario.Data
{
    public class WebApiProprietarioContext : DbContext
    {
        public WebApiProprietarioContext (DbContextOptions<WebApiProprietarioContext> options)
            : base(options)
        {
        }

        public DbSet<WebApiProprietario.Models.Proprietario> Proprietario { get; set; }
    }
}
