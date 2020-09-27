using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Fabricante
{
    public class FabricanteIndexViewModel
    {
        public List<FabricanteViewModel> Fabricantes { get; set; }
    }

    public class FabricanteViewModel
    {
        public string Id { get; set; }
        public string Logo { get; set; }
        public string Nome { get; set; }
    }
}
