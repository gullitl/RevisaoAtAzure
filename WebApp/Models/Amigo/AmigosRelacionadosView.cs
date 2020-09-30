using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models.Amigo
{
    public class AmigosRelacionadosView
    {
        public AmigoView Amigo { get; set; }
        public List<AmigoView> TodosAmigos { get; set; } = new List<AmigoView>();
        public List<string> AmigosRelacionadosIds { get; set; } = new List<string>();

    }
}
