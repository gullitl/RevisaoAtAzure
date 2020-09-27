using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProprietario.Models
{
    public class Proprietario
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public List<Carro> Carros { get; set; }
    }

    public class Carro
    {
        public string Id { get; set; }
        public string ProprietarioId { get; set; }
    }
}
