using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApp.Models.Estado
{
    public class EstadoView
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Campo Nome obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [JsonIgnore]
        public Pais.PaisView Pais { get; set; }

        [Required(ErrorMessage = "Campo Pais obrigatório.")]
        [ForeignKey("Pais")]
        [DisplayName("Pais")]
        public string PaisId { get; set; }
        
        [JsonIgnore]
        [Required(ErrorMessage = "Campo Bandeira obrigatório.")]
        [DisplayName("Bandeira")]
        public IFormFile LogoFile { get; set; }

        [DisplayName("Bandeira")]
        public string FotoBandeira { get; set; }
    }
}
