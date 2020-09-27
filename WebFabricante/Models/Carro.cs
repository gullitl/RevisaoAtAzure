using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFabricante.Models
{
    public class Carro
    {
        public string Id { get; set; }
        public string FabricanteId { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string Foto { get; set; }
    }
}
