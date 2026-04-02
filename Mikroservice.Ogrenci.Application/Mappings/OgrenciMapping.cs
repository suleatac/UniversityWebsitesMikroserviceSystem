using AutoMapper;

namespace Mikroservice.Ogrenci.Application.Mappings
{
    public class OgrenciMapping : Profile
    {
        public OgrenciMapping()
        {
            CreateMap<Microservice.Ogrenci.Domain.Entities.Ogrenci, Microservice.Ogrenci.Domain.Entities.Ogrenci>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
