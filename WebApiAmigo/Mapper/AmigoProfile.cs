using AutoMapper;
using WebApiAmigo.Controllers;
using WebApiAmigo.Models;

namespace WebApiAmigo.Mapper
{
    public class AmigoProfile : Profile
    {
        public AmigoProfile()
        {
            CreateMap<Amigo, AmigoResponse>();
            CreateMap<AmigoRequest, Amigo>();
        }
    }
}
