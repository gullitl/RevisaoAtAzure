using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApp.Models.Estado;

namespace WebApp.Models.Pais
{
    public class PaisView
    {
        [Key]
        public string PaisId { get; set; }

        [Required(ErrorMessage = "Campo Nome Completo obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [JsonIgnore]
        public IFormFile LogoFile { get; set; }

        [DisplayName("Bandeira")]
        public string FotoBandeira { get; set; }

        public List<EstadoView> Estados { get; set; }

        
    }
}
