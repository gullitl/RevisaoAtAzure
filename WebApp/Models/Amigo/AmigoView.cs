using Microsoft.AspNetCore.Http;

namespace WebApp.Models.Amigo
{
    public class AmigoView
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public IFormFile FotoFile { get; set; }
        public string Foto { get; set; }
    }
}
