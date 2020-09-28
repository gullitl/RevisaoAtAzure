namespace WebApp.Models.Home
{
    public class HomeIndexViewModel
    {
        public int QtdAmigos { get; set; }
        public int QtdPaises { get; set; }
        public int QtdEstados { get; set; }

        public bool IsValid => QtdAmigos >= 0 && QtdPaises >= 0 && QtdEstados >= 0;

    }
}
