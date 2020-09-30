using System;
using System.Collections.Generic;

namespace WebApiAmigo.Models
{
    public class Amigo
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public string PaisId { get; set; }
        public string EstadoId { get; set; }
        public string Foto { get; set; }
        public List<Amigo> AmigosRelacionados { get; set; }

    }
}
