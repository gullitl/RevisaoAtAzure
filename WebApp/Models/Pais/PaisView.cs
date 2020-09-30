using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApp.Models.Pais
{
    public class PaisView
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Campo Nome Completo obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Campo Bandeira Completo obrigatório.")]
        [DisplayName("Bandeira")]
        public IFormFile LogoFile { get; set; }

        [DisplayName("Bandeira")]
        public string FotoBandeira { get; set; }

        public List<Estado.EstadoView> Estados { get; set; }

        
    }
}
