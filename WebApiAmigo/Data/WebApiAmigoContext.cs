using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiAmigo.Controllers;
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
    }
}
