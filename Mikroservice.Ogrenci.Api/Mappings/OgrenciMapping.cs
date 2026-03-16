using AutoMapper;

namespace Mikroservice.Ogrenci.Api.Mappings
{
    public class OgrenciMapping:Profile
    {
        public OgrenciMapping()
        {
            CreateMap<Microservice.Ogrenci.Domain.Entities.Ogrenci, Microservice.Ogrenci.Domain.Entities.Ogrenci>().ReverseMap();
   
        }
    }
}
