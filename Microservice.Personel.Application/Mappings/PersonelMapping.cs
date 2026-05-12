using AutoMapper;
using Microservice.Personel.Application.Contracts.DTOs;

namespace Microservice.Personel.Application.Mappings
{
    public class PersonelMapping : Profile
    {
        public PersonelMapping()
        {
            CreateMap<Domain.Entities.Personel, Domain.Entities.Personel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Domain.Entities.Personel, PersonelDto>();
        }
    }
}
