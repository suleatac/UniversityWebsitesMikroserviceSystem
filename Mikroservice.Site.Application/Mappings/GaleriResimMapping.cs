using AutoMapper;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class GaleriResimMapping : Profile
    {
        public GaleriResimMapping()
        {
            CreateMap<GaleriResim, GaleriResimDto>();
            CreateMap<GaleriResim, GaleriResimDetailDto>();
        }
    }
}
