using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApp.Models.Estado;
using WebApp.Models.Pais;

namespace WebApp.Models.Amigo
{
    public class AmigoView
    {
        [Key]
        public string AmigoId { get; set; }

        [Required(ErrorMessage = "Campo Nome obrigatório.")]
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Sobrenome")]
        public string Sobrenome { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Telefone")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Campo Data Nascimento obrigatório.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Data Nascimento")]
        public DateTime DataNascimento { get; set; }

        [JsonIgnore]
        public Pais.PaisView Pais { get; set; }

        [Required(ErrorMessage = "Campo Pais obrigatório.")]
        [ForeignKey("Pais")]
        [DisplayName("Pais")]
        public string PaisId { get; set; }

        [JsonIgnore]
        public Estado.EstadoView Estado { get; set; }

        [Required(ErrorMessage = "Campo Estado obrigatório.")]
        [ForeignKey("Estado")]
        [DisplayName("Estado")]
        public string EstadoId { get; set; }

        [JsonIgnore]
        public IFormFile FotoFile { get; set; }

        [DisplayName("Foto")]
        public string Foto { get; set; }
    }
}
