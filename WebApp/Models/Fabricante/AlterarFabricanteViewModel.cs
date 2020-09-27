using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Fabricante
{
    public class AlterarFabricanteViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public IFormFile LogoFile { get; set; }
        public string Logo { get; set; }
    }
}
