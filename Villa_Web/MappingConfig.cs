using AutoMapper;
using Villa_Web.Models;
using Villa_Web.Models.DTO;

namespace Villa_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {


            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap();
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();

            CreateMap<VillaDTO, VillaUpdateDTO>().ReverseMap();
            CreateMap<VillaDTO, VillaCreateDTO>().ReverseMap();
        }
    }
    
    
}
