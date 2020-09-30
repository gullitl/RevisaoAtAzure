using System.Collections.Generic;

namespace WebApiAmigo.Models
{
    public class AmigosRelacionados
    {
        public Amigo Amigo { get; set; }
        public List<Amigo> TodosAmigos { get; set; }
        public List<string> AmigosRelacionadosIds { get; set; }
    }
}
