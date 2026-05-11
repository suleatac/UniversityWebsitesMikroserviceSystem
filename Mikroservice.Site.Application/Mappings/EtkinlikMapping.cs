using AutoMapper;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class EtkinlikMapping : Profile
    {
        public EtkinlikMapping()
        {
            CreateMap<Etkinlik, EtkinlikDto>();
            CreateMap<Etkinlik, EtkinlikDetailDto>();
        }
    }
}