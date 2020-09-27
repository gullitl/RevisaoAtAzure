using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
