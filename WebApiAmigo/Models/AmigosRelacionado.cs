using System.Collections.Generic;

namespace WebApiAmigo.Models
{
    public class AmigosRelacionado
    {
        public int Id { get; set; }
        public List<Amigo> Amigos { get; set; }
    }
}
