using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApp.Models.Pais;

namespace WebApp.Models.Estado
{
    public class EstadoView
    {
        [Key]
        public string EstadoId { get; set; }

        [Required(ErrorMessage = "Campo Nome obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }
        [JsonIgnore]

        public PaisView Pais { get; set; }

        [Required(ErrorMessage = "Campo Pais obrigatório.")]
        [ForeignKey("Pais")]
        [DisplayName("Pais")]
        public string PaisId { get; set; }
        
        [JsonIgnore]
        public IFormFile LogoFile { get; set; }

        [DisplayName("Bandeira")]
        public string FotoBandeira { get; set; }
    }
}
