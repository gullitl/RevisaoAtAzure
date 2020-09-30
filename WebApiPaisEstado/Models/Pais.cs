using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPaisEstado.Models
{
    public class Pais
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string FotoBandeira { get; set; }
        public List<Estado> Estados { get; set; }
    }
}
