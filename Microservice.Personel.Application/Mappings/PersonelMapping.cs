using AutoMapper;

namespace Microservice.Personel.Application.Mappings
{
    public class PersonelMapping : Profile
    {
        public PersonelMapping()
        {
            CreateMap<Domain.Entities.Personel, Domain.Entities.Personel>()
                .ForMember(dest => dest.id, opt => opt.Ignore());

        }
    }
}
