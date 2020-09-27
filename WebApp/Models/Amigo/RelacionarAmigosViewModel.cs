using System.Collections.Generic;

namespace WebApp.Models.Amigo
{
    public class RelacionarAmigosViewModel
    {
        public AmigoViewModel Amigo { get; set; }
        public List<AmigoViewModel> TodosAmigos { get; set; } = new List<AmigoViewModel>();
        public List<int> AmigosRelacionados { get; set; } = new List<int>();
    }
}
