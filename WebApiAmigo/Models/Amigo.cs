using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAmigo.Models
{
    public class Amigo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public List<Amigo> Amigos { get; set; }

        //public AmigosRelacionado Amigos { get; set; }
        //public void AdicionarAmigosRelacionados(List<Amigo> amigos)
        //{
        //    this.Amigos.Amigos = amigos;
        //}

    }
}
