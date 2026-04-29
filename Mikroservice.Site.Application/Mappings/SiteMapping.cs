using AutoMapper;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Mappings
{
    public class SiteMapping : Profile
    {
        public SiteMapping()
        {
            CreateMap<Domain.Entities.Site, SiteDto>().ReverseMap();
            CreateMap<Domain.Entities.Site, SiteDetailDto>().ReverseMap();
        }
    }
}
