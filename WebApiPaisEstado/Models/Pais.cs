using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPaisEstado.Models
{
    public class Pais
    {
        public string PaisId { get; set; }
        public string Nome { get; set; }
        public string FotoBandeira { get; set; }
        public List<Estado> Estados { get; set; }
    }
}
