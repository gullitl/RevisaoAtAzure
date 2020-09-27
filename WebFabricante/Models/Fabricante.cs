using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFabricante.Models
{
    public class Fabricante
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Logo { get; set; }
        public List<Carro> Carros { get; set; }
    }
}
