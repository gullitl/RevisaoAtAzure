using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace WebApp.Models.Amigo
{
    public class AmigoView
    {
        public string Id { get; set; }
        [DisplayName("Nome")]
        public string Nome { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        
        [JsonIgnore]
        public IFormFile FotoFile { get; set; }
        [DisplayName("Foto")]
        public string Foto { get; set; }
    }
}
